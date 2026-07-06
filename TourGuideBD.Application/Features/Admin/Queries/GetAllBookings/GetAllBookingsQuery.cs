using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Admin.Queries.GetAllBookings;

public class AdminBookingDto
{
    public int BookingId { get; set; }
    public string Status { get; set; } = string.Empty;

    // User info (who booked)
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;

    // Guide info
    public int GuideProfileId { get; set; }
    public string GuideName { get; set; } = string.Empty;
    public string GuidePhoneNumber { get; set; } = string.Empty;

    // Package info
    public int PackageId { get; set; }
    public string PackageTitle { get; set; } = string.Empty;

    // Booking details
    public DateTime TourDate { get; set; }
    public int NumberOfPeople { get; set; }
    public DateTime BookedAt { get; set; }

    // Payment info
    public decimal TotalAmount { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal GuideEarning { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? StripePaymentIntentId { get; set; }
    public string? StripeChargeId { get; set; }

    // Cancel info
    public string? CancellationReason { get; set; }
    public DateTime? CancelledAt { get; set; }
}

public class AdminBookingsSummaryDto
{
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public int ConfirmedBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int CancelledBookings { get; set; }

    public decimal TotalRevenue { get; set; }
    public decimal TotalPlatformFee { get; set; }
    public decimal TotalGuideEarnings { get; set; }

    public int PaidBookings { get; set; }
    public int UnpaidBookings { get; set; }

    public PaginatedList<AdminBookingDto> Bookings { get; set; } = null!;
}

public class GetAllBookingsQuery : IRequest<AdminBookingsSummaryDto>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    // Filters
    public BookingStatus? Status { get; set; }
    public bool? IsPaid { get; set; }
    public int? GuideProfileId { get; set; }
    public string? SearchTerm { get; set; } // User name/email/Guide name search
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class GetAllBookingsQueryValidator : AbstractValidator<GetAllBookingsQuery>
{
    public GetAllBookingsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}

public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, AdminBookingsSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetAllBookingsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminBookingsSummaryDto> Handle(
        GetAllBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Bookings
            .Include(b => b.TourPackage)
                .ThenInclude(p => p.GuideProfile)
            .AsQueryable();

        // Filters
        if (request.Status.HasValue)
        {
            query = query.Where(b => b.Status == request.Status.Value);
        }

        if (request.IsPaid.HasValue)
        {
            query = query.Where(b => b.IsPaid == request.IsPaid.Value);
        }

        if (request.GuideProfileId.HasValue)
        {
            query = query.Where(b =>
                b.TourPackage.GuideProfileId == request.GuideProfileId.Value);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(b => b.CreatedAt >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(b => b.CreatedAt <= request.ToDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(b =>
                b.TourPackage.GuideProfile.FullName.ToLower().Contains(term) ||
                b.TourPackage.Title.ToLower().Contains(term));
        }

        // Summary stats (filtered data থেকে before pagination)
        var allFiltered = await query.ToListAsync(cancellationToken);

        var summary = new AdminBookingsSummaryDto
        {
            TotalBookings = allFiltered.Count,
            PendingBookings = allFiltered.Count(b => b.Status == BookingStatus.Pending),
            ConfirmedBookings = allFiltered.Count(b => b.Status == BookingStatus.Confirmed),
            CompletedBookings = allFiltered.Count(b => b.Status == BookingStatus.Completed),
            CancelledBookings = allFiltered.Count(b =>
                b.Status == BookingStatus.CancelledByUser ||
                b.Status == BookingStatus.CancelledByGuide),

            TotalRevenue = allFiltered.Where(b => b.IsPaid).Sum(b => b.TotalAmount),
            TotalPlatformFee = allFiltered.Where(b => b.IsPaid).Sum(b => b.PlatformFee),
            TotalGuideEarnings = allFiltered.Where(b => b.IsPaid).Sum(b => b.GuideEarning),

            PaidBookings = allFiltered.Count(b => b.IsPaid),
            UnpaidBookings = allFiltered.Count(b => !b.IsPaid)
        };

        // User info লোড করো (separately — UserId string, ApplicationUser থেকে)
        var userIds = allFiltered.Select(b => b.UserId).Distinct().ToList();
        var users = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new { u.Id, u.FullName, u.Email })
            .ToListAsync(cancellationToken);

        // Pagination apply করো (in-memory, since summary already calculated)
        var pagedBookings = allFiltered
            .OrderByDescending(b => b.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b =>
            {
                var user = users.FirstOrDefault(u => u.Id == b.UserId);

                return new AdminBookingDto
                {
                    BookingId = b.Id,
                    Status = b.Status.ToString(),

                    UserId = b.UserId,
                    UserName = user?.FullName ?? "Unknown",
                    UserEmail = user?.Email ?? "Unknown",

                    GuideProfileId = b.TourPackage.GuideProfileId,
                    GuideName = b.TourPackage.GuideProfile.FullName,
                    GuidePhoneNumber = b.TourPackage.GuideProfile.PhoneNumber,

                    PackageId = b.TourPackageId,
                    PackageTitle = b.TourPackage.Title,

                    TourDate = b.TourDate,
                    NumberOfPeople = b.NumberOfPeople,
                    BookedAt = b.CreatedAt,

                    TotalAmount = b.TotalAmount,
                    PlatformFee = b.PlatformFee,
                    GuideEarning = b.GuideEarning,

                    IsPaid = b.IsPaid,
                    PaidAt = b.PaidAt,
                    StripePaymentIntentId = b.StripePaymentIntentId,
                    StripeChargeId = b.StripeChargeId,

                    CancellationReason = b.CancellationReason,
                    CancelledAt = b.CancelledAt
                };
            })
            .ToList();

        var totalPages = (int)Math.Ceiling(allFiltered.Count / (double)request.PageSize);

        summary.Bookings = new PaginatedList<AdminBookingDto>(
            pagedBookings, allFiltered.Count, request.PageNumber, request.PageSize);

        return summary;
    }
}