using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Admin.Queries.GetAllBookings;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Admin.Queries.GetBookingDetail;

public class GetBookingDetailQuery : IRequest<AdminBookingDto>
{
    public int BookingId { get; set; }
}

public class GetBookingDetailQueryValidator : AbstractValidator<GetBookingDetailQuery>
{
    public GetBookingDetailQueryValidator()
    {
        RuleFor(x => x.BookingId).GreaterThan(0);
    }
}

public class GetBookingDetailQueryHandler : IRequestHandler<GetBookingDetailQuery, AdminBookingDto>
{
    private readonly IApplicationDbContext _context;

    public GetBookingDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminBookingDto> Handle(
        GetBookingDetailQuery request,
        CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings
            .Include(b => b.TourPackage)
                .ThenInclude(p => p.GuideProfile)
            .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

        if (booking == null)
            throw new NotFoundException("Booking", request.BookingId);

        var user = await _context.Users
            .Where(u => u.Id == booking.UserId)
            .Select(u => new { u.FullName, u.Email })
            .FirstOrDefaultAsync(cancellationToken);

        return new AdminBookingDto
        {
            BookingId = booking.Id,
            Status = booking.Status.ToString(),

            UserId = booking.UserId,
            UserName = user?.FullName ?? "Unknown",
            UserEmail = user?.Email ?? "Unknown",

            GuideProfileId = booking.TourPackage.GuideProfileId,
            GuideName = booking.TourPackage.GuideProfile.FullName,
            GuidePhoneNumber = booking.TourPackage.GuideProfile.PhoneNumber,

            PackageId = booking.TourPackageId,
            PackageTitle = booking.TourPackage.Title,

            TourDate = booking.TourDate,
            NumberOfPeople = booking.NumberOfPeople,
            BookedAt = booking.CreatedAt,

            TotalAmount = booking.TotalAmount,
            PlatformFee = booking.PlatformFee,
            GuideEarning = booking.GuideEarning,
            IsPaid = booking.IsPaid,
            PaidAt = booking.PaidAt,
            StripePaymentIntentId = booking.StripePaymentIntentId,
            StripeChargeId = booking.StripeChargeId,

            CancellationReason = booking.CancellationReason,
            CancelledAt = booking.CancelledAt
        };
    }
}