using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.ConfirmBookingPayment;

public class ConfirmBookingPaymentCommand : IRequest<Unit>
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string ChargeId { get; set; } = string.Empty;
}

public class ConfirmBookingPaymentCommandHandler : IRequestHandler<ConfirmBookingPaymentCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IStripeService _stripeService;

    public ConfirmBookingPaymentCommandHandler(
        IApplicationDbContext context,
        IStripeService stripeService)
    {
        _context = context;
        _stripeService = stripeService;
    }

    public async Task<Unit> Handle(
        ConfirmBookingPaymentCommand request,
        CancellationToken cancellationToken)
    {
        // Stripe-এ গিয়ে সত্যিই succeeded কিনা verify করুন
        var isConfirmed = await _stripeService.ConfirmPaymentAsync(
            request.PaymentIntentId, cancellationToken);

        if (!isConfirmed)
        {
            throw new DomainValidationException("PaymentIntentId", "Payment has not succeeded on Stripe.");
        }

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b =>
                b.StripePaymentIntentId == request.PaymentIntentId, cancellationToken);

        if (booking == null)
        {
            throw new NotFoundException("Booking", request.PaymentIntentId);
        }

        // Already confirmed থাকলে দ্বিতীয়বার update করার দরকার নেই (webhook + client call দুটোই আসতে পারে)
        if (booking.IsPaid)
        {
            return Unit.Value;
        }

        booking.IsPaid = true;
        booking.PaidAt = DateTime.UtcNow;
        booking.Status = BookingStatus.Confirmed;
        booking.StripeChargeId = request.ChargeId;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}