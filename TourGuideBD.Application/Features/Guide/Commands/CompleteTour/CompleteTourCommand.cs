using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.CompleteTour;

public class CompleteTourCommand : IRequest<Unit>
{
    public int BookingId { get; set; }
    public string GuideUserId { get; set; } = string.Empty;
}

public class CompleteTourCommandValidator : AbstractValidator<CompleteTourCommand>
{
    public CompleteTourCommandValidator()
    {
        RuleFor(x => x.BookingId).GreaterThan(0);
        RuleFor(x => x.GuideUserId).NotEmpty();
    }
}

public class CompleteTourCommandHandler : IRequestHandler<CompleteTourCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public CompleteTourCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CompleteTourCommand request, CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings
            .Include(b => b.TourPackage)
                .ThenInclude(p => p.GuideProfile)
            .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

        if (booking == null)
            throw new NotFoundException("Booking", request.BookingId);

        if (booking.TourPackage.GuideProfile.UserId != request.GuideUserId)
            throw new ForbiddenAccessException("You can only complete your own bookings.");

        if (booking.Status != BookingStatus.Confirmed)
            throw new DomainValidationException("BookingId",
                "Only confirmed bookings can be completed.");

        booking.Status = BookingStatus.Completed;
        booking.UpdatedAt = DateTime.UtcNow;

        // Guide profile stats update
        var guide = booking.TourPackage.GuideProfile;
        guide.TotalToursCompleted += 1;

        // Revenue record create করো — 3 দিন পর Available হবে
        _context.GuideRevenues.Add(new GuideRevenue
        {
            GuideProfileId = guide.Id,
            BookingId = booking.Id,
            TotalAmount = booking.TotalAmount,
            PlatformFee = booking.PlatformFee,
            GuideEarning = booking.GuideEarning,
            Status = RevenueStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}