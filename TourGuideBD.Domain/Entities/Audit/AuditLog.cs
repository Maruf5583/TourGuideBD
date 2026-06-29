using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Audit;

public class AuditLog : BaseEntity
{
    public string? UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // Create, Update, Delete, Approve, Reject, Ban...
    public string EntityName { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}