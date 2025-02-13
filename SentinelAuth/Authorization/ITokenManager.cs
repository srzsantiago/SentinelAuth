using SentinelAuth.Models;

namespace SentinelAuth.Authorization;

/// <summary>
/// Interface for a TokenManager that generates a signed Json Web Token (JWT) containing unsensitive user data.
/// </summary>
public interface ITokenManager
{
    /// <summary>
    /// Generate a signed Json Web Token (JWT) containing unsensitive user data. It makes use of the given JwtConfig.
    /// </summary>
    /// <returns>string JWT token</returns>
    string GenerateJwtToken(SentinelUser user);
}