using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Trip;

public class TransportRate : BaseEntity
{
    public int TransportTypeId { get; set; }
    public TransportType TransportType { get; set; } = null!;

    // Cost per KM in BDT
    public decimal RatePerKm { get; set; }

    // Average speed in km/h - used for travel time estimation
    public double AverageSpeedKmh { get; set; }

    // Minimum fare (e.g., CNG minimum charge)
    public decimal MinimumFare { get; set; }

    public bool IsActive { get; set; } = true;
}