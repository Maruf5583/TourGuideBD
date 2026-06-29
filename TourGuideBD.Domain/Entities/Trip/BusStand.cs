using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Domain.Entities.Trip;

public class BusStand : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;
    public int DistrictId { get; set; }
    public District District { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsMainStand { get; set; } = true;
}