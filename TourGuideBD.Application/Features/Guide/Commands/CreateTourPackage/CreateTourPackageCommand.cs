using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Commands.CreateTourPackage;

public class CreateTourPackageCommand : IRequest<int>
{
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

    // Availability dates
    public List<AvailabilityInput> Availabilities { get; set; } = new();
}

public class AvailabilityInput
{
    public DateTime Date { get; set; }
    public int MaxBookings { get; set; }
}

public class CreateTourPackageCommandValidator : AbstractValidator<CreateTourPackageCommand>
{
    public CreateTourPackageCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.PricePerPerson).GreaterThan(0);
        RuleFor(x => x.MaxPeople).InclusiveBetween(1, 50);
        RuleFor(x => x.DurationDays).InclusiveBetween(1, 30);
        RuleFor(x => x.MeetingPoint).NotEmpty().MaximumLength(300);
        RuleFor(x => x.PlaceIds).NotEmpty().WithMessage("At least one place required.");
        RuleFor(x => x.MeetingLat).InclusiveBetween(-90, 90);
        RuleFor(x => x.MeetingLng).InclusiveBetween(-180, 180);
    }
}

public class CreateTourPackageCommandHandler : IRequestHandler<CreateTourPackageCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTourPackageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTourPackageCommand request, CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId && g.IsActive, cancellationToken);

        if (guide == null)
        {
            throw new NotFoundException("GuideProfile", request.GuideUserId);
        }

        var package = new TourPackage
        {
            GuideProfileId = guide.Id,
            Title = request.Title,
            Description = request.Description,
            PricePerPerson = request.PricePerPerson,
            MaxPeople = request.MaxPeople,
            DurationDays = request.DurationDays,
            IncludesFood = request.IncludesFood,
            IncludesTransport = request.IncludesTransport,
            IncludesAccommodation = request.IncludesAccommodation,
            AdditionalIncludes = request.AdditionalIncludes,
            MeetingPoint = request.MeetingPoint,
            MeetingLat = request.MeetingLat,
            MeetingLng = request.MeetingLng,
            PlaceIds = JsonSerializer.Serialize(request.PlaceIds),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Availability add করো
        foreach (var avail in request.Availabilities)
        {
            package.Availabilities.Add(new GuideAvailability
            {
                AvailableDate = avail.Date,
                MaxBookings = avail.MaxBookings,
                CurrentBookings = 0,
                IsAvailable = true
            });
        }

        _context.TourPackages.Add(package);
        await _context.SaveChangesAsync(cancellationToken);

        return package.Id;
    }
}