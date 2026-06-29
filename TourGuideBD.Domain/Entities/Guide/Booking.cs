using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Guide;

public class Booking : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public int TourPackageId { get; set; }
    public TourPackage TourPackage { get; set; } = null!;

    public DateTime TourDate { get; set; }
    public int NumberOfPeople { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal PlatformFee { get; set; }     // 10% commission
    public decimal GuideEarning { get; set; }    // 90%

    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    // Stripe
    public string? StripePaymentIntentId { get; set; }
    public string? StripeChargeId { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidAt { get; set; }

    public string? CancellationReason { get; set; }
    public DateTime? CancelledAt { get; set; }

    public GuideReview? Review { get; set; }
}