using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Admin.Commands.ProcessWithdrawal;

public class ProcessWithdrawalCommand : IRequest<Unit>
{
    public int WithdrawalId { get; set; }
    public bool IsApproved { get; set; }
    public string? AdminNote { get; set; }
    public string? TransactionReference { get; set; }
    public string AdminUserId { get; set; } = string.Empty;
}

public class ProcessWithdrawalCommandValidator : AbstractValidator<ProcessWithdrawalCommand>
{
    public ProcessWithdrawalCommandValidator()
    {
        RuleFor(x => x.WithdrawalId).GreaterThan(0);
        RuleFor(x => x.AdminNote).MaximumLength(500);
        RuleFor(x => x.TransactionReference).MaximumLength(200);
    }
}

public class ProcessWithdrawalCommandHandler : IRequestHandler<ProcessWithdrawalCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ProcessWithdrawalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        ProcessWithdrawalCommand request,
        CancellationToken cancellationToken)
    {
        var withdrawal = await _context.WithdrawalRequests
            .FirstOrDefaultAsync(w => w.Id == request.WithdrawalId, cancellationToken);

        if (withdrawal == null)
            throw new NotFoundException("WithdrawalRequest", request.WithdrawalId);

        if (withdrawal.Status == WithdrawalStatus.Completed
            || withdrawal.Status == WithdrawalStatus.Rejected)
            throw new DomainValidationException("WithdrawalId",
                "This withdrawal has already been processed.");

        if (request.IsApproved)
        {
            withdrawal.Status = WithdrawalStatus.Completed;
            withdrawal.TransactionReference = request.TransactionReference;

            // Revenue status update — PaidOut করো
            var availableDate = DateTime.UtcNow.AddMinutes(-5);
            var revenues = await _context.GuideRevenues
                .Where(r => r.GuideProfileId == withdrawal.GuideProfileId
                    && (r.Status == RevenueStatus.Available
                        || (r.Status == RevenueStatus.Pending
                            && r.CreatedAt <= availableDate)))
                .OrderBy(r => r.CreatedAt)
                .ToListAsync(cancellationToken);

            decimal remaining = withdrawal.RequestedAmount;
            foreach (var revenue in revenues)
            {
                if (remaining <= 0) break;

                if (revenue.GuideEarning <= remaining)
                {
                    revenue.Status = RevenueStatus.PaidOut;
                    revenue.PaidOutAt = DateTime.UtcNow;
                    remaining -= revenue.GuideEarning;
                }
                else
                {
                    // Partial — এই revenue টা split করার দরকার নেই এখন
                    break;
                }
            }
        }
        else
        {
            withdrawal.Status = WithdrawalStatus.Rejected;
        }

        withdrawal.AdminNote = request.AdminNote;
        withdrawal.ProcessedByUserId = request.AdminUserId;
        withdrawal.ProcessedAt = DateTime.UtcNow;
        withdrawal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}