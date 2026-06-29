using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Domain.Entities.Trip;

public class DistrictBusRoute : BaseEntity
{
    public int FromDistrictId { get; set; }
    public District FromDistrict { get; set; } = null!;

    public int ToDistrictId { get; set; }
    public District ToDistrict { get; set; } = null!;

    public decimal BusCost { get; set; }
    public int BusTimeMinutes { get; set; }

    /// <summary>
    /// Bidirectional — same cost both ways
    /// </summary>
    public bool IsBidirectional { get; set; } = true;
}