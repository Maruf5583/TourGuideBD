using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Reviews;

public class ReviewReport : AuditableEntity
{
    public int ReviewId { get; set; }
    public Review Review { get; set; } = null!;

    public string ReportedByUserId { get; set; } = string.Empty;
    public ApplicationUser ReportedByUser { get; set; } = null!;

    public string Reason { get; set; } = string.Empty;
    public ReportStatus Status { get; set; } = ReportStatus.Open;

    public string? ResolvedByUserId { get; set; }
    public string? ResolutionNote { get; set; }
}