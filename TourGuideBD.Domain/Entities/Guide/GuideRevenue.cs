using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Guide;

public class GuideRevenue : BaseEntity
{
    public int GuideProfileId { get; set; }
    public GuideProfile GuideProfile { get; set; } = null!;

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public decimal TotalAmount { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal GuideEarning { get; set; }

    public RevenueStatus Status { get; set; } = RevenueStatus.Pending;
    public DateTime? PaidOutAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}