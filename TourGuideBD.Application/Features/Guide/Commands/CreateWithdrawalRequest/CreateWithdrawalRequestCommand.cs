using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.CreateWithdrawalRequest;

public class CreateWithdrawalRequestCommand : IRequest<int>
{
    public string GuideUserId { get; set; } = string.Empty;
    public int PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
}

public class CreateWithdrawalRequestCommandValidator : AbstractValidator<CreateWithdrawalRequestCommand>
{
    public CreateWithdrawalRequestCommandValidator()
    {
        RuleFor(x => x.PaymentMethodId).GreaterThan(0);
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(500).WithMessage("Minimum withdrawal amount is 500 BDT.");
    }
}

public class CreateWithdrawalRequestCommandHandler : IRequestHandler<CreateWithdrawalRequestCommand, int>
{
    private readonly IApplicationDbContext _context;
    private const decimal ProcessingFeePercent = 0.01m; // 1%

    public CreateWithdrawalRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        CreateWithdrawalRequestCommand request,
        CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        // Pending withdrawal আছে কিনা check
        var hasPending = await _context.WithdrawalRequests
            .AnyAsync(w => w.GuideProfileId == guide.Id
                && (w.Status == WithdrawalStatus.Pending
                    || w.Status == WithdrawalStatus.Processing),
                cancellationToken);

        if (hasPending)
            throw new DomainValidationException("WithdrawalRequest",
                "You already have a pending withdrawal request. Wait for it to be processed.");

        // Available balance check
        // Available balance check
        var availableDate = DateTime.UtcNow.AddMinutes(-5);

        var eligibleBucket = await _context.GuideRevenues
            .Where(r => r.GuideProfileId == guide.Id
                && r.Status != RevenueStatus.PaidOut
                && (r.Status == RevenueStatus.Available
                    || (r.Status == RevenueStatus.Pending && r.CreatedAt <= availableDate)))
            .SumAsync(r => r.GuideEarning, cancellationToken);

        var completedWithdrawals = await _context.WithdrawalRequests
            .Where(w => w.GuideProfileId == guide.Id && w.Status == WithdrawalStatus.Completed)
            .SumAsync(w => w.RequestedAmount, cancellationToken);

        var availableBalance = eligibleBucket - completedWithdrawals;

        if (request.Amount > availableBalance)
            throw new DomainValidationException("Amount",
                $"Insufficient balance. Available: {availableBalance} BDT.");

        // Payment method verify
        var paymentMethod = await _context.GuidePaymentMethods
            .FirstOrDefaultAsync(m =>
                m.Id == request.PaymentMethodId && m.GuideProfileId == guide.Id,
                cancellationToken);

        if (paymentMethod == null)
            throw new NotFoundException("PaymentMethod", request.PaymentMethodId);

        var processingFee = Math.Round(request.Amount * ProcessingFeePercent, 2);
        var netAmount = request.Amount - processingFee;

        var withdrawal = new WithdrawalRequest
        {
            GuideProfileId = guide.Id,
            PaymentMethodId = request.PaymentMethodId,
            RequestedAmount = request.Amount,
            ProcessingFee = processingFee,
            NetAmount = netAmount,
            Status = WithdrawalStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.WithdrawalRequests.Add(withdrawal);
        await _context.SaveChangesAsync(cancellationToken);

        return withdrawal.Id;
    }
}