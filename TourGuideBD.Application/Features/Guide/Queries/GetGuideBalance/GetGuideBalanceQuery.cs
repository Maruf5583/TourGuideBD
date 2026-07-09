using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Queries.GetGuideBalance;

public class GuideBalanceDto
{
    public decimal TotalEarned { get; set; }
    public decimal PendingAmount { get; set; }
    public decimal AvailableAmount { get; set; }
    public decimal WithdrawnAmount { get; set; }
    public decimal PendingWithdrawal { get; set; }
}

public class GetGuideBalanceQuery : IRequest<GuideBalanceDto>
{
    public string GuideUserId { get; set; } = string.Empty;
}

public class GetGuideBalanceQueryValidator : AbstractValidator<GetGuideBalanceQuery>
{
    public GetGuideBalanceQueryValidator()
    {
        RuleFor(x => x.GuideUserId).NotEmpty();
    }
}

public class GetGuideBalanceQueryHandler : IRequestHandler<GetGuideBalanceQuery, GuideBalanceDto>
{
    private readonly IApplicationDbContext _context;

    public GetGuideBalanceQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GuideBalanceDto> Handle(
     GetGuideBalanceQuery request,
     CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        var availableDate = DateTime.UtcNow.AddMinutes(-5);

        var revenues = await _context.GuideRevenues
            .Where(r => r.GuideProfileId == guide.Id)
            .ToListAsync(cancellationToken);

        var pendingWithdrawals = await _context.WithdrawalRequests
            .Where(w => w.GuideProfileId == guide.Id
                && (w.Status == WithdrawalStatus.Pending
                    || w.Status == WithdrawalStatus.Processing))
            .SumAsync(w => w.RequestedAmount, cancellationToken);

        // ✅ NOTUN: Completed withdrawals-er total ber koro
        var completedWithdrawals = await _context.WithdrawalRequests
            .Where(w => w.GuideProfileId == guide.Id
                && w.Status == WithdrawalStatus.Completed)
            .SumAsync(w => w.RequestedAmount, cancellationToken);

        var eligibleBucket = revenues
            .Where(r => r.Status != RevenueStatus.PaidOut
                && (r.Status == RevenueStatus.Available
                    || (r.Status == RevenueStatus.Pending && r.CreatedAt <= availableDate)))
            .Sum(r => r.GuideEarning);

        return new GuideBalanceDto
        {
            TotalEarned = revenues.Sum(r => r.GuideEarning),

            PendingAmount = revenues
                .Where(r => r.Status == RevenueStatus.Pending
                    && r.CreatedAt > availableDate)
                .Sum(r => r.GuideEarning),

            // ✅ FIX: eligible bucket theke completed + pending withdrawals dutai substract koro
            AvailableAmount = eligibleBucket - completedWithdrawals - pendingWithdrawals,

            // ✅ FIX: WithdrawnAmount ekhon completed withdrawals theke ashbe (revenue.PaidOut theke na)
            WithdrawnAmount = completedWithdrawals,

            PendingWithdrawal = pendingWithdrawals
        };
    }
}
