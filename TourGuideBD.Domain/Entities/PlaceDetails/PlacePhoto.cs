using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Domain.Entities.PlaceDetails;

public class PlacePhoto : BaseEntity
{
    public int PlaceId { get; set; }
    public Place Place { get; set; } = null!;

    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public bool IsCover { get; set; } = false;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}