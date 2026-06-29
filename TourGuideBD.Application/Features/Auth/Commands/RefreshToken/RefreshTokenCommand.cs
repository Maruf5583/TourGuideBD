using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Auth.Common;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<AuthResponseDto>
{
    public string UserId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public RefreshTokenCommandHandler(
        IJwtTokenService jwtTokenService,
        IIdentityService identityService,
        IApplicationDbContext context)
    {
        _jwtTokenService = jwtTokenService;
        _identityService = identityService;
        _context = context;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validatedUserId = await _jwtTokenService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

        if (validatedUserId == null)
        {
            throw new ForbiddenAccessException("Invalid or expired refresh token.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == validatedUserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("ApplicationUser", validatedUserId);
        }

        var isBanned = await _identityService.IsUserBannedAsync(validatedUserId);
        if (isBanned)
        {
            throw new ForbiddenAccessException("Your account has been banned.");
        }

        var roles = await _identityService.GetUserRolesAsync(validatedUserId);

        // Rotation: revoke old, issue new (GenerateTokensAsync overwrites Redis entry anyway)
        var tokens = await _jwtTokenService.GenerateTokensAsync(validatedUserId, user.Email!, roles);

        return new AuthResponseDto
        {
            UserId = validatedUserId,
            Email = user.Email!,
            FullName = user.FullName,
            Roles = roles.ToList(),
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            AccessTokenExpiresAt = tokens.AccessTokenExpiresAt
        };
    }
}