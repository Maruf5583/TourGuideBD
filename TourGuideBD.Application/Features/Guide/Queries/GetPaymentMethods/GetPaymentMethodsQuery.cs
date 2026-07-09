using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Queries.GetPaymentMethods;

public class PaymentMethodDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? MobileNumber { get; set; }
    public string? BankName { get; set; }
    public string? AccountName { get; set; }
    public string? AccountNumber { get; set; }
    public string? BranchName { get; set; }
    public string? RoutingNumber { get; set; }
    public bool IsDefault { get; set; }
}

public class GetPaymentMethodsQuery : IRequest<List<PaymentMethodDto>>
{
    public string GuideUserId { get; set; } = string.Empty;
}

public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, List<PaymentMethodDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPaymentMethodsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PaymentMethodDto>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        return await _context.GuidePaymentMethods
            .Where(m => m.GuideProfileId == guide.Id)
            .OrderByDescending(m => m.IsDefault)
            .ThenByDescending(m => m.CreatedAt)
            .Select(m => new PaymentMethodDto
            {
                Id = m.Id,
                Type = m.Type.ToString(),
                MobileNumber = m.MobileNumber,
                BankName = m.BankName,
                AccountName = m.AccountName,
                AccountNumber = m.AccountNumber,
                BranchName = m.BranchName,
                RoutingNumber = m.RoutingNumber,
                IsDefault = m.IsDefault
            })
            .ToListAsync(cancellationToken);
    }
}