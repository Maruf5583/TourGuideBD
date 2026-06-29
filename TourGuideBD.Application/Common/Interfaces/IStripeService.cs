namespace TourGuideBD.Application.Common.Interfaces;

public class CreatePaymentIntentResult
{
    public string ClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
}

public interface IStripeService
{
    Task<CreatePaymentIntentResult> CreatePaymentIntentAsync(
        decimal amount,
        string currency,
        string description,
        string customerEmail,
        Dictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);

    Task<bool> ConfirmPaymentAsync(
        string paymentIntentId,
        CancellationToken cancellationToken = default);

    Task<bool> RefundPaymentAsync(
        string chargeId,
        decimal? amount = null,
        CancellationToken cancellationToken = default);
}