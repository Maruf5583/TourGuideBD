using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Queries.GetWithdrawalHistory;

public class WithdrawalHistoryDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;

    // Amount info
    public decimal RequestedAmount { get; set; }
    public decimal ProcessingFee { get; set; }
    public decimal NetAmount { get; set; }

    // Payment method info
    public string PaymentMethodType { get; set; } = string.Empty;
    public string PaymentMethodDisplay { get; set; } = string.Empty;

    // Transaction info
    public string? TransactionReference { get; set; }
    public string? AdminNote { get; set; }

    // Dates
    public DateTime RequestedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

public class GetWithdrawalHistoryQuery : IRequest<PaginatedList<WithdrawalHistoryDto>>
{
    public string GuideUserId { get; set; } = string.Empty;
    public WithdrawalStatus? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetWithdrawalHistoryQueryValidator : AbstractValidator<GetWithdrawalHistoryQuery>
{
    public GetWithdrawalHistoryQueryValidator()
    {
        RuleFor(x => x.GuideUserId).NotEmpty();
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetWithdrawalHistoryQueryHandler
    : IRequestHandler<GetWithdrawalHistoryQuery, PaginatedList<WithdrawalHistoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetWithdrawalHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<WithdrawalHistoryDto>> Handle(
        GetWithdrawalHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        var query = _context.WithdrawalRequests
            .Include(w => w.PaymentMethod)
            .Where(w => w.GuideProfileId == guide.Id);

        if (request.Status.HasValue)
            query = query.Where(w => w.Status == request.Status.Value);

        var projected = query
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => new WithdrawalHistoryDto
            {
                Id = w.Id,
                Status = w.Status.ToString(),
                RequestedAmount = w.RequestedAmount,
                ProcessingFee = w.ProcessingFee,
                NetAmount = w.NetAmount,
                PaymentMethodType = w.PaymentMethod.Type.ToString(),
                PaymentMethodDisplay = w.PaymentMethod.Type == PaymentMethodType.BKash
                    ? $"bKash — {w.PaymentMethod.MobileNumber}"
                    : w.PaymentMethod.Type == PaymentMethodType.Nagad
                        ? $"Nagad — {w.PaymentMethod.MobileNumber}"
                        : $"{w.PaymentMethod.BankName} — {w.PaymentMethod.AccountNumber}",
                TransactionReference = w.TransactionReference,
                AdminNote = w.AdminNote,
                RequestedAt = w.CreatedAt,
                ProcessedAt = w.ProcessedAt
            });

        return await PaginatedList<WithdrawalHistoryDto>.CreateAsync(
            projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}