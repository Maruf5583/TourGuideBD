using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Guide;

public class GuideLiveLocation : BaseEntity
{
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public string GuideUserId { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public bool IsSharing { get; set; } = false;
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}