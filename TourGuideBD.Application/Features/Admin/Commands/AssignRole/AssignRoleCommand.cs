using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Exceptions;
using ValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Admin.Commands.AssignRole;

public class AssignRoleCommand : IRequest<Unit>, IAuditableRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool Assign { get; set; } = true;

    public string ActionName => Assign ? "AssignRole" : "RemoveRole";
    public string EntityName => nameof(ApplicationUser);
    public string? EntityId => UserId;
}

public class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(r => new[] { "User", "Moderator", "Admin", "TourGuide" }.Contains(r))
            .WithMessage("Role must be one of: User, Moderator, Admin, TourGuide");
    }
}

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Unit>
{
    private readonly IIdentityService _identityService;

    public AssignRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Unit> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var result = request.Assign
            ? await _identityService.AddUserToRoleAsync(request.UserId, request.Role)
            : await _identityService.RemoveUserFromRoleAsync(request.UserId, request.Role);

        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(e =>
                new FluentValidation.Results.ValidationFailure(nameof(request.Role), e)));
        }

        return Unit.Value;
    }
}