using Microsoft.Extensions.Configuration;
using Stripe;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Infrastructure.ExternalServices;

public class StripeService : IStripeService
{
    public StripeService(IConfiguration configuration)
    {
        StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"]
            ?? throw new InvalidOperationException("Stripe:SecretKey is not configured.");
    }

    public async Task<CreatePaymentIntentResult> CreatePaymentIntentAsync(
        decimal amount,
        string currency,
        string description,
        string customerEmail,
        Dictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Stripe uses cents
            Currency = currency.ToLower(),
            Description = description,
            ReceiptEmail = customerEmail,
            Metadata = metadata ?? new Dictionary<string, string>(),
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options, cancellationToken: cancellationToken);

        return new CreatePaymentIntentResult
        {
            ClientSecret = intent.ClientSecret,
            PaymentIntentId = intent.Id
        };
    }

    public async Task<bool> ConfirmPaymentAsync(
        string paymentIntentId,
        CancellationToken cancellationToken = default)
    {
        var service = new PaymentIntentService();
        var intent = await service.GetAsync(paymentIntentId, cancellationToken: cancellationToken);
        return intent.Status == "succeeded";
    }

    public async Task<bool> RefundPaymentAsync(
        string chargeId,
        decimal? amount = null,
        CancellationToken cancellationToken = default)
    {
        var options = new RefundCreateOptions
        {
            Charge = chargeId,
            Amount = amount.HasValue ? (long)(amount.Value * 100) : null
        };

        var service = new RefundService();
        var refund = await service.CreateAsync(options, cancellationToken: cancellationToken);

        return refund.Status == "succeeded";
    }
}