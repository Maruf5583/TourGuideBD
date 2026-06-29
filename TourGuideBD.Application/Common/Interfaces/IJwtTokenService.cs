namespace TourGuideBD.Application.Common.Interfaces;

public class TokenResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAt { get; set; }
}

public interface IJwtTokenService
{
    Task<TokenResult> GenerateTokensAsync(string userId, string email, IList<string> roles);

    /// <summary>
    /// Validates refresh token against Redis store; returns userId if valid.
    /// </summary>
    Task<string?> ValidateRefreshTokenAsync(string userId, string refreshToken);

    Task RevokeRefreshTokenAsync(string userId);
}