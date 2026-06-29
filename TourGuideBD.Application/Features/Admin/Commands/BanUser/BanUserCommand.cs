using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Exceptions;
using ValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Admin.Commands.BanUser;

public class BanUserCommand : IRequest<Unit>, IAuditableRequest
{
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// true = Ban, false = Unban
    /// </summary>
    public bool IsBanned { get; set; }

    public string ActionName => IsBanned ? "BanUser" : "UnbanUser";
    public string EntityName => nameof(ApplicationUser);
    public string? EntityId => UserId;
}

public class BanUserCommandValidator : AbstractValidator<BanUserCommand>
{
    public BanUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class BanUserCommandHandler : IRequestHandler<BanUserCommand, Unit>
{
    private readonly IIdentityService _identityService;

    public BanUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Unit> Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.SetBanStatusAsync(request.UserId, request.IsBanned);

        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(e =>
                new FluentValidation.Results.ValidationFailure(nameof(request.UserId), e)));
        }

        return Unit.Value;
    }
}