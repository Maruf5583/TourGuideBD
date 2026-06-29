using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Domain.Entities.PlaceDetails;

public class PlaceCategoryMap
{
    public int PlaceId { get; set; }
    public Place Place { get; set; } = null!;

    public int PlaceCategoryId { get; set; }
    public PlaceCategory PlaceCategory { get; set; } = null!;
}