using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;
    private readonly IDistributedCache _cache;
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(IOptions<JwtSettings> settings, IDistributedCache cache, ILogger<JwtTokenService> logger)
    {
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task<TokenResult> GenerateTokensAsync(string userId, string email, IList<string> roles)
    {
        var accessTokenExpiry = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpiryMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: accessTokenExpiry,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = GenerateRefreshToken();

        // Store refresh token in Redis with rotation expiry — but don't let registration/login fail if Redis is down
        var cacheKey = GetRefreshTokenCacheKey(userId);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(_settings.RefreshTokenExpiryDays)
        };

        try
        {
            await _cache.SetStringAsync(cacheKey, refreshToken, options);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis unavailable — refresh token not cached for user {UserId}. Token issued without persistence.", userId);
        }

        return new TokenResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = accessTokenExpiry
        };
    }

    public async Task<string?> ValidateRefreshTokenAsync(string userId, string refreshToken)
    {
        var cacheKey = GetRefreshTokenCacheKey(userId);

        string? storedToken;
        try
        {
            storedToken = await _cache.GetStringAsync(cacheKey);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis unavailable — cannot validate refresh token for user {UserId}.", userId);
            return null;
        }

        if (string.IsNullOrEmpty(storedToken) || storedToken != refreshToken)
        {
            return null;
        }

        return userId;
    }

    public async Task RevokeRefreshTokenAsync(string userId)
    {
        var cacheKey = GetRefreshTokenCacheKey(userId);
        try
        {
            await _cache.RemoveAsync(cacheKey);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis unavailable — could not revoke refresh token for user {UserId}.", userId);
        }
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private static string GetRefreshTokenCacheKey(string userId) => $"refresh-token:{userId}";
}