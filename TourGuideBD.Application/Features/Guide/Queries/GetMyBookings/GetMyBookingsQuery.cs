using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Guide.Queries.GetMyBookings;

public class MyBookingDto
{
    public int BookingId { get; set; }
    public string Status { get; set; } = string.Empty;

    // Package info
    public int PackageId { get; set; }
    public string PackageTitle { get; set; } = string.Empty;
    public string PackageDescription { get; set; } = string.Empty;

    // Guide info
    public int GuideProfileId { get; set; }
    public string GuideName { get; set; } = string.Empty;
    public string GuidePhotoUrl { get; set; } = string.Empty;
    public string GuidePhoneNumber { get; set; } = string.Empty;
    public double GuideAverageRating { get; set; }

    // Booking details
    public DateTime TourDate { get; set; }
    public int NumberOfPeople { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime BookedAt { get; set; }

    // Meeting info
    public string MeetingPoint { get; set; } = string.Empty;
    public double MeetingLat { get; set; }
    public double MeetingLng { get; set; }
    public string MeetingGoogleMapsUrl { get; set; } = string.Empty;

    // What's included
    public bool IncludesFood { get; set; }
    public bool IncludesTransport { get; set; }
    public bool IncludesAccommodation { get; set; }

    // Cancel info
    public string? CancellationReason { get; set; }
    public DateTime? CancelledAt { get; set; }

    // Review status
    public bool HasReviewed { get; set; }

    // Can perform actions?
    public bool CanCancel { get; set; }
    public bool CanReview { get; set; }
}

public class GetMyBookingsQuery : IRequest<PaginatedList<MyBookingDto>>
{
    public string UserId { get; set; } = string.Empty;
    public BookingStatus? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetMyBookingsQueryValidator : AbstractValidator<GetMyBookingsQuery>
{
    public GetMyBookingsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetMyBookingsQueryHandler : IRequestHandler<GetMyBookingsQuery, PaginatedList<MyBookingDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMyBookingsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<MyBookingDto>> Handle(
        GetMyBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Bookings
            .Include(b => b.TourPackage)
                .ThenInclude(p => p.GuideProfile)
            .Include(b => b.Review)
            .Where(b => b.UserId == request.UserId);

        if (request.Status.HasValue)
        {
            query = query.Where(b => b.Status == request.Status.Value);
        }

        var projected = query
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new MyBookingDto
            {
                BookingId = b.Id,
                Status = b.Status.ToString(),

                PackageId = b.TourPackageId,
                PackageTitle = b.TourPackage.Title,
                PackageDescription = b.TourPackage.Description,

                GuideProfileId = b.TourPackage.GuideProfileId,
                GuideName = b.TourPackage.GuideProfile.FullName,
                GuidePhotoUrl = b.TourPackage.GuideProfile.ProfilePhotoUrl,
                GuidePhoneNumber = b.TourPackage.GuideProfile.PhoneNumber,
                GuideAverageRating = b.TourPackage.GuideProfile.AverageRating,

                TourDate = b.TourDate,
                NumberOfPeople = b.NumberOfPeople,
                TotalAmount = b.TotalAmount,
                IsPaid = b.IsPaid,
                PaidAt = b.PaidAt,
                BookedAt = b.CreatedAt,

                MeetingPoint = b.TourPackage.MeetingPoint,
                MeetingLat = b.TourPackage.MeetingLat,
                MeetingLng = b.TourPackage.MeetingLng,
                MeetingGoogleMapsUrl =
                    $"https://www.google.com/maps/search/?api=1&query={b.TourPackage.MeetingLat},{b.TourPackage.MeetingLng}",

                IncludesFood = b.TourPackage.IncludesFood,
                IncludesTransport = b.TourPackage.IncludesTransport,
                IncludesAccommodation = b.TourPackage.IncludesAccommodation,

                CancellationReason = b.CancellationReason,
                CancelledAt = b.CancelledAt,

                HasReviewed = b.Review != null,

                // Cancel করা যাবে যদি Pending/Confirmed থাকে এবং tour date এখনো না এসে থাকে
                CanCancel = (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed)
                    && b.TourDate > DateTime.UtcNow,

                // Review দেওয়া যাবে যদি Completed থাকে এবং আগে review না দেওয়া থাকে
                CanReview = b.Status == BookingStatus.Completed && b.Review == null
            });

        return await PaginatedList<MyBookingDto>.CreateAsync(
            projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}