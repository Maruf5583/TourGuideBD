using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.DeletePaymentMethod;

public class DeletePaymentMethodCommand : IRequest<Unit>
{
    public int PaymentMethodId { get; set; }
    public string GuideUserId { get; set; } = string.Empty;
}

public class DeletePaymentMethodCommandValidator : AbstractValidator<DeletePaymentMethodCommand>
{
    public DeletePaymentMethodCommandValidator()
    {
        RuleFor(x => x.PaymentMethodId).GreaterThan(0);
        RuleFor(x => x.GuideUserId).NotEmpty();
    }
}

public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeletePaymentMethodCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeletePaymentMethodCommand request,
        CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        var method = await _context.GuidePaymentMethods
            .FirstOrDefaultAsync(m =>
                m.Id == request.PaymentMethodId &&
                m.GuideProfileId == guide.Id,
                cancellationToken);

        if (method == null)
            throw new NotFoundException("PaymentMethod", request.PaymentMethodId);

        // Pending withdrawal এ use হচ্ছে কিনা check করো
        var isUsedInPendingWithdrawal = await _context.WithdrawalRequests
            .AnyAsync(w =>
                w.PaymentMethodId == request.PaymentMethodId &&
                (w.Status == WithdrawalStatus.Pending ||
                 w.Status == WithdrawalStatus.Processing),
                cancellationToken);

        if (isUsedInPendingWithdrawal)
        {
            throw new DomainValidationException("PaymentMethodId",
                "Cannot delete this payment method. It is being used in a pending withdrawal request.");
        }

        _context.GuidePaymentMethods.Remove(method);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}