using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Commands.AddPaymentMethod;

public class AddPaymentMethodCommand : IRequest<int>
{
    public string GuideUserId { get; set; } = string.Empty;
    public PaymentMethodType Type { get; set; }

    // bKash/Nagad
    public string? MobileNumber { get; set; }

    // Bank
    public string? BankName { get; set; }
    public string? AccountName { get; set; }
    public string? AccountNumber { get; set; }
    public string? BranchName { get; set; }
    public string? RoutingNumber { get; set; }

    public bool IsDefault { get; set; } = false;
}

public class AddPaymentMethodCommandValidator : AbstractValidator<AddPaymentMethodCommand>
{
    public AddPaymentMethodCommandValidator()
    {
        RuleFor(x => x.Type).IsInEnum();

        When(x => x.Type == PaymentMethodType.BKash || x.Type == PaymentMethodType.Nagad, () =>
        {
            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile number is required for bKash/Nagad.")
                .Matches(@"^01[3-9]\d{8}$").WithMessage("Invalid Bangladesh mobile number.");
        });

        When(x => x.Type == PaymentMethodType.Bank, () =>
        {
            RuleFor(x => x.BankName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.AccountName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.AccountNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.BranchName).NotEmpty().MaximumLength(100);
        });
    }
}

public class AddPaymentMethodCommandHandler : IRequestHandler<AddPaymentMethodCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AddPaymentMethodCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(AddPaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        // Default হলে পুরনোটা unset করো
        if (request.IsDefault)
        {
            var existing = await _context.GuidePaymentMethods
                .Where(m => m.GuideProfileId == guide.Id && m.IsDefault)
                .ToListAsync(cancellationToken);

            foreach (var m in existing)
                m.IsDefault = false;
        }

        var method = new GuidePaymentMethod
        {
            GuideProfileId = guide.Id,
            Type = request.Type,
            MobileNumber = request.MobileNumber,
            BankName = request.BankName,
            AccountName = request.AccountName,
            AccountNumber = request.AccountNumber,
            BranchName = request.BranchName,
            RoutingNumber = request.RoutingNumber,
            IsDefault = request.IsDefault,
            CreatedAt = DateTime.UtcNow
        };

        _context.GuidePaymentMethods.Add(method);
        await _context.SaveChangesAsync(cancellationToken);

        return method.Id;
    }
}