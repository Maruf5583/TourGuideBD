using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Queries.GetGuideRevenue;

public class GuideRevenueDto
{
    // Summary
    public decimal TotalEarned { get; set; }
    public decimal PendingPayout { get; set; }
    public decimal PaidOut { get; set; }
    public int TotalBookings { get; set; }
    public int CompletedTours { get; set; }
    public double AverageRating { get; set; }

    // Monthly breakdown
    public List<MonthlyRevenueDto> MonthlyBreakdown { get; set; } = new();

    // Package wise stats
    public List<PackageRevenueDto> PackageStats { get; set; } = new();
}

public class MonthlyRevenueDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public decimal Earned { get; set; }
    public int Bookings { get; set; }
}

public class PackageRevenueDto
{
    public int PackageId { get; set; }
    public string PackageTitle { get; set; } = string.Empty;
    public decimal TotalEarned { get; set; }
    public int TotalBookings { get; set; }
    public decimal PricePerPerson { get; set; }
}

public class GetGuideRevenueQuery : IRequest<GuideRevenueDto>
{
    public string GuideUserId { get; set; } = string.Empty;
    public int? Year { get; set; }
}

public class GetGuideRevenueQueryHandler : IRequestHandler<GetGuideRevenueQuery, GuideRevenueDto>
{
    private readonly IApplicationDbContext _context;

    public GetGuideRevenueQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GuideRevenueDto> Handle(
        GetGuideRevenueQuery request,
        CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
        {
            throw new NotFoundException("GuideProfile", request.GuideUserId);
        }

        // Revenue query
        var revenueQuery = _context.GuideRevenues
            .Where(r => r.GuideProfileId == guide.Id);

        if (request.Year.HasValue)
        {
            revenueQuery = revenueQuery.Where(r =>
                r.CreatedAt.Year == request.Year.Value);
        }

        var revenues = await revenueQuery
            .Include(r => r.Booking)
            .ThenInclude(b => b.TourPackage)
            .ToListAsync(cancellationToken);

        // Summary
        var totalEarned = revenues.Sum(r => r.GuideEarning);
        var pendingPayout = revenues
            .Where(r => r.Status == RevenueStatus.Available)
            .Sum(r => r.GuideEarning);
        var paidOut = revenues
            .Where(r => r.Status == RevenueStatus.PaidOut)
            .Sum(r => r.GuideEarning);

        var completedBookings = await _context.Bookings
            .CountAsync(b =>
                b.TourPackage.GuideProfileId == guide.Id &&
                b.Status == BookingStatus.Completed,
                cancellationToken);

        var totalBookings = await _context.Bookings
            .CountAsync(b => b.TourPackage.GuideProfileId == guide.Id, cancellationToken);

        // Monthly breakdown
        var monthly = revenues
            .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
            .Select(g => new MonthlyRevenueDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                MonthName = new DateTime(g.Key.Year, g.Key.Month, 1)
                    .ToString("MMMM yyyy"),
                Earned = g.Sum(r => r.GuideEarning),
                Bookings = g.Count()
            })
            .OrderByDescending(m => m.Year)
            .ThenByDescending(m => m.Month)
            .ToList();

        // Package stats
        var packageStats = revenues
            .GroupBy(r => new
            {
                r.Booking.TourPackageId,
                r.Booking.TourPackage.Title,
                r.Booking.TourPackage.PricePerPerson
            })
            .Select(g => new PackageRevenueDto
            {
                PackageId = g.Key.TourPackageId,
                PackageTitle = g.Key.Title,
                TotalEarned = g.Sum(r => r.GuideEarning),
                TotalBookings = g.Count(),
                PricePerPerson = g.Key.PricePerPerson
            })
            .OrderByDescending(p => p.TotalEarned)
            .ToList();

        return new GuideRevenueDto
        {
            TotalEarned = Math.Round(totalEarned, 2),
            PendingPayout = Math.Round(pendingPayout, 2),
            PaidOut = Math.Round(paidOut, 2),
            TotalBookings = totalBookings,
            CompletedTours = completedBookings,
            AverageRating = guide.AverageRating,
            MonthlyBreakdown = monthly,
            PackageStats = packageStats
        };
    }
}