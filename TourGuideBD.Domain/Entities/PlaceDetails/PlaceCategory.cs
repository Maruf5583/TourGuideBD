using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.PlaceDetails;

public class PlaceCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public PlaceCategoryEnum CategoryType { get; set; }

    public ICollection<PlaceCategoryMap> CategoryMaps { get; set; } = new List<PlaceCategoryMap>();
}