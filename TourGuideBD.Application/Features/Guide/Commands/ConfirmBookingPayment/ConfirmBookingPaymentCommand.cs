using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Commands.ConfirmBookingPayment;

public class ConfirmBookingPaymentCommand : IRequest<Unit>
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string ChargeId { get; set; } = string.Empty;
}

public class ConfirmBookingPaymentCommandHandler : IRequestHandler<ConfirmBookingPaymentCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ConfirmBookingPaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        ConfirmBookingPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b =>
                b.StripePaymentIntentId == request.PaymentIntentId, cancellationToken);

        if (booking == null)
        {
            throw new NotFoundException("Booking", request.PaymentIntentId);
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