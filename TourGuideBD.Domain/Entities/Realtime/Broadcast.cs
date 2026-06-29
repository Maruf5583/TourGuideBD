using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Domain.Entities.Realtime;

public class Broadcast : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    // Null = system-wide
    public int? DistrictId { get; set; }
    public District? District { get; set; }

    public string SentByUserId { get; set; } = string.Empty;
}