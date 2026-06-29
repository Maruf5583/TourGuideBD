using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Realtime;

public class LiveVisitor : BaseEntity
{
    public int PlaceId { get; set; }
    public Place Place { get; set; } = null!;

    public int CurrentCount { get; set; } = 0;
    public CrowdLevel CrowdLevel { get; set; } = CrowdLevel.Low;

    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}