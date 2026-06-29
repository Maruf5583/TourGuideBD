using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Location;

public class Upazila : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;

    public int DistrictId { get; set; }
    public District District { get; set; } = null!;

    public ICollection<Place> Places { get; set; } = new List<Place>();
}