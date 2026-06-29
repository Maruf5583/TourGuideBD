using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Domain.Entities.Users;

public class VisitHistory : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int PlaceId { get; set; }
    public Place Place { get; set; } = null!;

    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
}