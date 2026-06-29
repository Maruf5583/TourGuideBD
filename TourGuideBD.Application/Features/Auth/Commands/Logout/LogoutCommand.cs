using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Application.Features.Auth.Commands.Logout;

public class LogoutCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
}

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IJwtTokenService _jwtTokenService;

    public LogoutCommandHandler(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _jwtTokenService.RevokeRefreshTokenAsync(request.UserId);
        return Unit.Value;
    }
}