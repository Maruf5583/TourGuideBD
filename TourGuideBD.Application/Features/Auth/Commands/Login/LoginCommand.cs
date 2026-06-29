using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Auth.Common;
using TourGuideBD.Domain.Exceptions;
using ValidationException = TourGuideBD.Domain.Exceptions.ValidationException;


namespace TourGuideBD.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IApplicationDbContext _context;

    public LoginCommandHandler(
        IIdentityService identityService,
        IJwtTokenService jwtTokenService,
        IApplicationDbContext context)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
        _context = context;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var isValidPassword = await _identityService.CheckPasswordAsync(request.Email, request.Password);

        if (!isValidPassword)
        {
            throw new ValidationException("Email", "Invalid email or password.");
        }

        var userId = await _identityService.GetUserIdByEmailAsync(request.Email);
        if (userId == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Users.ApplicationUser), request.Email);
        }

        var isBanned = await _identityService.IsUserBannedAsync(userId);
        if (isBanned)
        {
            throw new ForbiddenAccessException("Your account has been banned. Please contact support.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        var roles = await _identityService.GetUserRolesAsync(userId);
        var tokens = await _jwtTokenService.GenerateTokensAsync(userId, request.Email, roles);

        return new AuthResponseDto
        {
            UserId = userId,
            Email = request.Email,
            FullName = user?.FullName ?? string.Empty,
            Roles = roles.ToList(),
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            AccessTokenExpiresAt = tokens.AccessTokenExpiresAt
        };
    }
}