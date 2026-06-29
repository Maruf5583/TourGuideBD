using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Admin.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Admin.Queries.GetAnalytics;

public class GetAnalyticsQuery : IRequest<AnalyticsDashboardDto>
{
}

public class GetAnalyticsQueryHandler : IRequestHandler<GetAnalyticsQuery, AnalyticsDashboardDto>
{
    private readonly IApplicationDbContext _context;

    public GetAnalyticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnalyticsDashboardDto> Handle(GetAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var totalUsers = await _context.Users.CountAsync(cancellationToken);
        var totalPlaces = await _context.Places.CountAsync(p => p.ApprovalStatus == ApprovalStatus.Approved, cancellationToken);
        var pendingPlaces = await _context.Places.CountAsync(p => p.ApprovalStatus == ApprovalStatus.Pending, cancellationToken);
        var pendingReviews = await _context.Reviews.CountAsync(r => r.Status == ApprovalStatus.Pending, cancellationToken);
        var openReports = await _context.ReviewReports.CountAsync(r => r.Status == ReportStatus.Open, cancellationToken);

        var districts = await _context.Districts
            .Select(d => new { d.Id, d.Name })
            .ToListAsync(cancellationToken);

        var placeCounts = await _context.Places
            .Where(p => p.ApprovalStatus == ApprovalStatus.Approved)
            .GroupBy(p => p.DistrictId)
            .Select(g => new { DistrictId = g.Key, Count = g.Count(), AvgRating = g.Average(p => p.AverageRating) })
            .ToListAsync(cancellationToken);

        var visitCounts = await _context.VisitHistories
            .GroupBy(v => v.Place.DistrictId)
            .Select(g => new { DistrictId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var checkInCounts = await _context.CheckIns
            .GroupBy(c => c.Place.DistrictId)
            .Select(g => new { DistrictId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var reviewCounts = await _context.Reviews
            .Where(r => r.Status == ApprovalStatus.Approved)
            .GroupBy(r => r.Place.DistrictId)
            .Select(g => new { DistrictId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var districtStats = districts
            .Select(d =>
            {
                var placeInfo = placeCounts.FirstOrDefault(p => p.DistrictId == d.Id);
                return new DistrictVisitStatDto
                {
                    DistrictId = d.Id,
                    DistrictName = d.Name,
                    TotalPlaces = placeInfo?.Count ?? 0,
                    TotalVisits = visitCounts.FirstOrDefault(v => v.DistrictId == d.Id)?.Count ?? 0,
                    TotalCheckIns = checkInCounts.FirstOrDefault(c => c.DistrictId == d.Id)?.Count ?? 0,
                    TotalReviews = reviewCounts.FirstOrDefault(r => r.DistrictId == d.Id)?.Count ?? 0,
                    AverageRating = Math.Round(placeInfo?.AvgRating ?? 0, 2)
                };
            })
            .Where(d => d.TotalPlaces > 0)
            .OrderByDescending(d => d.TotalVisits)
            .ToList();

        return new AnalyticsDashboardDto
        {
            TotalUsers = totalUsers,
            TotalPlaces = totalPlaces,
            PendingPlaces = pendingPlaces,
            PendingReviews = pendingReviews,
            OpenReports = openReports,
            DistrictStats = districtStats
        };
    }
}