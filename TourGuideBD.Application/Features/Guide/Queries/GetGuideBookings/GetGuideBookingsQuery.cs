using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Queries.GetGuideBookings;

public class GuideBookingDto
{
    public int BookingId { get; set; }
    public string Status { get; set; } = string.Empty;

    // User info
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;

    // Package info
    public int PackageId { get; set; }
    public string PackageTitle { get; set; } = string.Empty;

    // Booking details
    public DateTime TourDate { get; set; }
    public int NumberOfPeople { get; set; }
    public DateTime BookedAt { get; set; }

    // Payment
    public decimal TotalAmount { get; set; }
    public decimal GuideEarning { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }

    // Actions
    public bool CanComplete { get; set; }
    public bool CanCancel { get; set; }

    // Review
    public bool HasReview { get; set; }
    public int? ReviewRating { get; set; }
    public string? ReviewComment { get; set; }
}

public class GetGuideBookingsQuery : IRequest<PaginatedList<GuideBookingDto>>
{
    public string GuideUserId { get; set; } = string.Empty;
    public BookingStatus? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetGuideBookingsQueryValidator : AbstractValidator<GetGuideBookingsQuery>
{
    public GetGuideBookingsQueryValidator()
    {
        RuleFor(x => x.GuideUserId).NotEmpty();
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetGuideBookingsQueryHandler
    : IRequestHandler<GetGuideBookingsQuery, PaginatedList<GuideBookingDto>>
{
    private readonly IApplicationDbContext _context;

    public GetGuideBookingsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GuideBookingDto>> Handle(
        GetGuideBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        var query = _context.Bookings
            .Include(b => b.TourPackage)
            .Include(b => b.Review)
            .Where(b => b.TourPackage.GuideProfileId == guide.Id);

        if (request.Status.HasValue)
            query = query.Where(b => b.Status == request.Status.Value);

        var userIds = await query.Select(b => b.UserId).Distinct().ToListAsync(cancellationToken);
        var users = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new { u.Id, u.FullName, u.Email })
            .ToListAsync(cancellationToken);

        var projected = query
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new
            {
                b.Id,
                b.Status,
                b.UserId,
                b.TourPackageId,
                PackageTitle = b.TourPackage.Title,
                b.TourDate,
                b.NumberOfPeople,
                b.CreatedAt,
                b.TotalAmount,
                b.GuideEarning,
                b.IsPaid,
                b.PaidAt,
                HasReview = b.Review != null,
                ReviewRating = b.Review != null ? b.Review.OverallRating : (int?)null,
                ReviewComment = b.Review != null ? b.Review.Comment : null
            });

        var paged = await PaginatedList<dynamic>.CreateAsync(
            projected, request.PageNumber, request.PageSize, cancellationToken);

        var items = paged.Items.Select(b =>
        {
            var user = users.FirstOrDefault(u => u.Id == b.UserId);
            return new GuideBookingDto
            {
                BookingId = b.Id,
                Status = b.Status.ToString(),
                UserName = user?.FullName ?? "Unknown",
                UserEmail = user?.Email ?? "Unknown",
                PackageId = b.TourPackageId,
                PackageTitle = b.PackageTitle,
                TourDate = b.TourDate,
                NumberOfPeople = b.NumberOfPeople,
                BookedAt = b.CreatedAt,
                TotalAmount = b.TotalAmount,
                GuideEarning = b.GuideEarning,
                IsPaid = b.IsPaid,
                PaidAt = b.PaidAt,
                HasReview = b.HasReview,
                ReviewRating = b.ReviewRating,
                ReviewComment = b.ReviewComment,
                CanComplete = b.Status == BookingStatus.Confirmed
                    && b.TourDate.Date <= DateTime.UtcNow.Date,
                CanCancel = (b.Status == BookingStatus.Pending
                    || b.Status == BookingStatus.Confirmed)
                    && b.TourDate > DateTime.UtcNow
            };
        }).ToList();

        return new PaginatedList<GuideBookingDto>(
            items, paged.TotalCount, request.PageNumber, request.PageSize);
    }
}