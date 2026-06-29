namespace TourGuideBD.Application.Features.Admin.Common;

public class UserManagementDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsBanned { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class DistrictVisitStatDto
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; } = string.Empty;
    public int TotalPlaces { get; set; }
    public int TotalVisits { get; set; }
    public int TotalCheckIns { get; set; }
    public int TotalReviews { get; set; }
    public double AverageRating { get; set; }
}

public class AnalyticsDashboardDto
{
    public int TotalUsers { get; set; }
    public int TotalPlaces { get; set; }
    public int PendingPlaces { get; set; }
    public int PendingReviews { get; set; }
    public int OpenReports { get; set; }
    public List<DistrictVisitStatDto> DistrictStats { get; set; } = new();
}

public class AuditLogDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public DateTime Timestamp { get; set; }
}