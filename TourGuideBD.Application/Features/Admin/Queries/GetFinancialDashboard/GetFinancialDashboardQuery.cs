using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Admin.Queries.GetFinancialDashboard;

public class GuideEarningStatDto
{
    public int GuideProfileId { get; set; }
    public string GuideName { get; set; } = string.Empty;
    public string GuidePhotoUrl { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public int TotalToursCompleted { get; set; }
    public decimal TotalEarned { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal GuideEarning { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal WithdrawnAmount { get; set; }
    public int TotalReviews { get; set; }

    // Monthly breakdown
    public List<MonthlyEarningDto> MonthlyEarnings { get; set; } = new();
}

public class MonthlyEarningDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal PlatformFee { get; set; }
    public int Bookings { get; set; }
}

public class WithdrawalSummaryDto
{
    public int Id { get; set; }
    public string GuideName { get; set; } = string.Empty;
    public decimal RequestedAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string PaymentMethodType { get; set; } = string.Empty;
    public string? MobileNumber { get; set; }
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; }
    public string? TransactionReference { get; set; }
}

public class FinancialDashboardDto
{
    // Platform totals
    public decimal TotalPlatformRevenue { get; set; }
    public decimal ThisMonthRevenue { get; set; }
    public decimal TotalGuideEarnings { get; set; }
    public decimal PendingWithdrawals { get; set; }
    public decimal TotalPaidOut { get; set; }

    // Booking stats
    public int TotalBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int PendingBookings { get; set; }

    // Monthly platform revenue
    public List<MonthlyEarningDto> MonthlyRevenue { get; set; } = new();

    // Per guide stats
    public List<GuideEarningStatDto> GuideStats { get; set; } = new();

    // Pending withdrawals
    public List<WithdrawalSummaryDto> PendingWithdrawalList { get; set; } = new();
}

public class GetFinancialDashboardQuery : IRequest<FinancialDashboardDto>
{
    public int? Year { get; set; }
}

public class GetFinancialDashboardQueryHandler
    : IRequestHandler<GetFinancialDashboardQuery, FinancialDashboardDto>
{
    private readonly IApplicationDbContext _context;

    public GetFinancialDashboardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinancialDashboardDto> Handle(
        GetFinancialDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var year = request.Year ?? DateTime.UtcNow.Year;
        var thisMonth = DateTime.UtcNow;

        // All revenues
        var allRevenues = await _context.GuideRevenues
            .Include(r => r.GuideProfile)
            .ToListAsync(cancellationToken);

        // This month revenues
        var thisMonthRevenues = allRevenues
            .Where(r => r.CreatedAt.Year == thisMonth.Year
                && r.CreatedAt.Month == thisMonth.Month)
            .ToList();

        // Pending withdrawals
        var pendingWithdrawals = await _context.WithdrawalRequests
            .Include(w => w.GuideProfile)
            .Include(w => w.PaymentMethod)
            .Where(w => w.Status == WithdrawalStatus.Pending
                || w.Status == WithdrawalStatus.Processing)
            .OrderBy(w => w.CreatedAt)
            .ToListAsync(cancellationToken);

        // Booking stats
        var bookings = await _context.Bookings.ToListAsync(cancellationToken);

        // Monthly revenue for selected year
        var monthlyRevenue = allRevenues
            .Where(r => r.CreatedAt.Year == year)
            .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
            .Select(g => new MonthlyEarningDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy"),
                Revenue = g.Sum(r => r.TotalAmount),
                PlatformFee = g.Sum(r => r.PlatformFee),
                Bookings = g.Count()
            })
            .OrderBy(m => m.Year).ThenBy(m => m.Month)
            .ToList();

        // Per guide stats
        var availableDate = DateTime.UtcNow.AddDays(-3);
        var guides = await _context.GuideProfiles
            .Where(g => g.IsActive)
            .ToListAsync(cancellationToken);

        var guideStats = guides.Select(g =>
        {
            var gRevenues = allRevenues.Where(r => r.GuideProfileId == g.Id).ToList();
            var gWithdrawn = _context.WithdrawalRequests
                .Where(w => w.GuideProfileId == g.Id && w.Status == WithdrawalStatus.Completed)
                .Sum(w => w.RequestedAmount);

            var available = gRevenues
                .Where(r => r.Status == RevenueStatus.Available
                    || (r.Status == RevenueStatus.Pending && r.CreatedAt <= availableDate))
                .Sum(r => r.GuideEarning);

            var monthly = gRevenues
                .Where(r => r.CreatedAt.Year == year)
                .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
                .Select(grp => new MonthlyEarningDto
                {
                    Year = grp.Key.Year,
                    Month = grp.Key.Month,
                    MonthName = new DateTime(grp.Key.Year, grp.Key.Month, 1).ToString("MMMM yyyy"),
                    Revenue = grp.Sum(r => r.TotalAmount),
                    PlatformFee = grp.Sum(r => r.PlatformFee),
                    Bookings = grp.Count()
                })
                .OrderBy(m => m.Month)
                .ToList();

            return new GuideEarningStatDto
            {
                GuideProfileId = g.Id,
                GuideName = g.FullName,
                GuidePhotoUrl = g.ProfilePhotoUrl,
                AverageRating = g.AverageRating,
                TotalToursCompleted = g.TotalToursCompleted,
                TotalReviews = g.TotalReviews,
                TotalEarned = gRevenues.Sum(r => r.TotalAmount),
                PlatformFee = gRevenues.Sum(r => r.PlatformFee),
                GuideEarning = gRevenues.Sum(r => r.GuideEarning),
                AvailableBalance = available,
                WithdrawnAmount = gWithdrawn,
                MonthlyEarnings = monthly
            };
        })
        .OrderByDescending(g => g.TotalEarned)
        .ToList();

        return new FinancialDashboardDto
        {
            TotalPlatformRevenue = allRevenues.Sum(r => r.PlatformFee),
            ThisMonthRevenue = thisMonthRevenues.Sum(r => r.PlatformFee),
            TotalGuideEarnings = allRevenues.Sum(r => r.GuideEarning),
            PendingWithdrawals = pendingWithdrawals.Sum(w => w.RequestedAmount),
            TotalPaidOut = _context.WithdrawalRequests
                .Where(w => w.Status == WithdrawalStatus.Completed)
                .Sum(w => w.RequestedAmount),

            TotalBookings = bookings.Count,
            CompletedBookings = bookings.Count(b => b.Status == BookingStatus.Completed),
            PendingBookings = bookings.Count(b => b.Status == BookingStatus.Pending),

            MonthlyRevenue = monthlyRevenue,
            GuideStats = guideStats,

            PendingWithdrawalList = pendingWithdrawals.Select(w => new WithdrawalSummaryDto
            {
                Id = w.Id,
                GuideName = w.GuideProfile.FullName,
                RequestedAmount = w.RequestedAmount,
                NetAmount = w.NetAmount,
                PaymentMethodType = w.PaymentMethod.Type.ToString(),
                MobileNumber = w.PaymentMethod.MobileNumber,
                BankName = w.PaymentMethod.BankName,
                AccountNumber = w.PaymentMethod.AccountNumber,
                Status = w.Status.ToString(),
                RequestedAt = w.CreatedAt,
                TransactionReference = w.TransactionReference
            }).ToList()
        };
    }
}