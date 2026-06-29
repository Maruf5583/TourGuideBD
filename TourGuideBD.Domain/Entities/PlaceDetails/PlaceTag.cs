using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Domain.Entities.PlaceDetails;

public class PlaceTag : BaseEntity
{
    public int PlaceId { get; set; }
    public Place Place { get; set; } = null!;

    public string Tag { get; set; } = string.Empty;
}