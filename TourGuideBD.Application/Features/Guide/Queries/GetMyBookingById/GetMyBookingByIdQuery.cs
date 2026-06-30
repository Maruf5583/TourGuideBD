using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Guide.Queries.GetMyBookings;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Queries.GetMyBookingById;

public class GetMyBookingByIdQuery : IRequest<MyBookingDto>
{
    public int BookingId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class GetMyBookingByIdQueryValidator : AbstractValidator<GetMyBookingByIdQuery>
{
    public GetMyBookingByIdQueryValidator()
    {
        RuleFor(x => x.BookingId).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class GetMyBookingByIdQueryHandler : IRequestHandler<GetMyBookingByIdQuery, MyBookingDto>
{
    private readonly IApplicationDbContext _context;

    public GetMyBookingByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MyBookingDto> Handle(
        GetMyBookingByIdQuery request,
        CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings
            .Include(b => b.TourPackage)
                .ThenInclude(p => p.GuideProfile)
            .Include(b => b.Review)
            .FirstOrDefaultAsync(b =>
                b.Id == request.BookingId && b.UserId == request.UserId,
                cancellationToken);

        if (booking == null)
            throw new NotFoundException("Booking", request.BookingId);

        return new MyBookingDto
        {
            BookingId = booking.Id,
            Status = booking.Status.ToString(),

            PackageId = booking.TourPackageId,
            PackageTitle = booking.TourPackage.Title,
            PackageDescription = booking.TourPackage.Description,

            GuideProfileId = booking.TourPackage.GuideProfileId,
            GuideName = booking.TourPackage.GuideProfile.FullName,
            GuidePhotoUrl = booking.TourPackage.GuideProfile.ProfilePhotoUrl,
            GuidePhoneNumber = booking.TourPackage.GuideProfile.PhoneNumber,
            GuideAverageRating = booking.TourPackage.GuideProfile.AverageRating,

            TourDate = booking.TourDate,
            NumberOfPeople = booking.NumberOfPeople,
            TotalAmount = booking.TotalAmount,
            IsPaid = booking.IsPaid,
            PaidAt = booking.PaidAt,
            BookedAt = booking.CreatedAt,

            MeetingPoint = booking.TourPackage.MeetingPoint,
            MeetingLat = booking.TourPackage.MeetingLat,
            MeetingLng = booking.TourPackage.MeetingLng,
            MeetingGoogleMapsUrl =
                $"https://www.google.com/maps/search/?api=1&query={booking.TourPackage.MeetingLat},{booking.TourPackage.MeetingLng}",

            IncludesFood = booking.TourPackage.IncludesFood,
            IncludesTransport = booking.TourPackage.IncludesTransport,
            IncludesAccommodation = booking.TourPackage.IncludesAccommodation,

            CancellationReason = booking.CancellationReason,
            CancelledAt = booking.CancelledAt,

            HasReviewed = booking.Review != null,

            CanCancel = (booking.Status == BookingStatus.Pending
                || booking.Status == BookingStatus.Confirmed)
                && booking.TourDate > DateTime.UtcNow,

            CanReview = booking.Status == BookingStatus.Completed && booking.Review == null
        };
    }
}