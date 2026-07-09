using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Guide;

public class WithdrawalRequest : AuditableEntity
{
    public int GuideProfileId { get; set; }
    public GuideProfile GuideProfile { get; set; } = null!;

    public int PaymentMethodId { get; set; }
    public GuidePaymentMethod PaymentMethod { get; set; } = null!;

    public decimal RequestedAmount { get; set; }
    public decimal ProcessingFee { get; set; }
    public decimal NetAmount { get; set; }

    public WithdrawalStatus Status { get; set; } = WithdrawalStatus.Pending;

    public string? AdminNote { get; set; }
    public string? ProcessedByUserId { get; set; }
    public DateTime? ProcessedAt { get; set; }

    public string? TransactionReference { get; set; }
}