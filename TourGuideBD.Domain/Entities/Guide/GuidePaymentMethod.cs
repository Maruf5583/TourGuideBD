using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Guide;

public class GuidePaymentMethod : BaseEntity
{
    public int GuideProfileId { get; set; }
    public GuideProfile GuideProfile { get; set; } = null!;

    public PaymentMethodType Type { get; set; }

    // bKash / Nagad
    public string? MobileNumber { get; set; }

    // Bank
    public string? BankName { get; set; }
    public string? AccountName { get; set; }
    public string? AccountNumber { get; set; }
    public string? BranchName { get; set; }
    public string? RoutingNumber { get; set; }

    public bool IsDefault { get; set; } = false;
    public bool IsVerified { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}