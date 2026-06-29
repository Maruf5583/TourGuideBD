using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.UpdateTourPackage;

public class UpdateTourPackageCommand : IRequest<Unit>
{
    public int PackageId { get; set; }
    public string GuideUserId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerPerson { get; set; }
    public int MaxPeople { get; set; }
    public int DurationDays { get; set; }

    public bool IncludesFood { get; set; }
    public bool IncludesTransport { get; set; }
    public bool IncludesAccommodation { get; set; }
    public string? AdditionalIncludes { get; set; }

    public string MeetingPoint { get; set; } = string.Empty;
    public double MeetingLat { get; set; }
    public double MeetingLng { get; set; }

    public List<int> PlaceIds { get; set; } = new();
    public bool IsActive { get; set; } = true;
}

public class UpdateTourPackageCommandValidator : AbstractValidator<UpdateTourPackageCommand>
{
    public UpdateTourPackageCommandValidator()
    {
        RuleFor(x => x.PackageId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.PricePerPerson).GreaterThan(0);
        RuleFor(x => x.MaxPeople).InclusiveBetween(1, 50);
        RuleFor(x => x.DurationDays).InclusiveBetween(1, 30);
        RuleFor(x => x.MeetingPoint).NotEmpty().MaximumLength(300);
        RuleFor(x => x.PlaceIds).NotEmpty();
        RuleFor(x => x.MeetingLat).InclusiveBetween(-90, 90);
        RuleFor(x => x.MeetingLng).InclusiveBetween(-180, 180);
    }
}

public class UpdateTourPackageCommandHandler : IRequestHandler<UpdateTourPackageCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateTourPackageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTourPackageCommand request, CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId && g.IsActive, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        var package = await _context.TourPackages
            .FirstOrDefaultAsync(p => p.Id == request.PackageId
                && p.GuideProfileId == guide.Id, cancellationToken);

        if (package == null)
            throw new NotFoundException("TourPackage", request.PackageId);

        // Active booking থাকলে price/people change করতে দেবো না
        var hasActiveBookings = await _context.Bookings
            .AnyAsync(b => b.TourPackageId == request.PackageId
                && (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed),
                cancellationToken);

        if (hasActiveBookings && request.PricePerPerson != package.PricePerPerson)
        {
            throw new DomainValidationException("PricePerPerson",
                "Cannot change price while there are active bookings.");
        }

        package.Title = request.Title;
        package.Description = request.Description;
        package.PricePerPerson = request.PricePerPerson;
        package.MaxPeople = request.MaxPeople;
        package.DurationDays = request.DurationDays;
        package.IncludesFood = request.IncludesFood;
        package.IncludesTransport = request.IncludesTransport;
        package.IncludesAccommodation = request.IncludesAccommodation;
        package.AdditionalIncludes = request.AdditionalIncludes;
        package.MeetingPoint = request.MeetingPoint;
        package.MeetingLat = request.MeetingLat;
        package.MeetingLng = request.MeetingLng;
        package.PlaceIds = JsonSerializer.Serialize(request.PlaceIds);
        package.IsActive = request.IsActive;
        package.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}