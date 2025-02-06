using System.IdentityModel.Tokens.Jwt;
using System.Text;
using SentinelAuth.Config;
using SentinelAuth.Interfaces;
using SentinelAuth.Models;
using SentinelAuth.Wrappers;

namespace SentinelAuth.Authorization;

public class TokenManager(JwtConfig config, IJwtWrapper jwtWrapper)
{
    private readonly JwtConfig _config = config;
    private readonly IJwtWrapper _jwtWrapper = jwtWrapper;

    // Constructor that allows setting a custom JwtConfig
    public TokenManager(JwtConfig config) : this(config, new JwtWrapper())
    {
    }

    /// <summary>
    /// Generate a signed Json Web Token (JWT) containing unsensitive user data. It makes use of the given JwtConfig.
    /// </summary>
    /// <returns>string JWT token</returns>
    public string GenerateJwtToken(SentinelUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var secretKey = Encoding.UTF8.GetBytes(_config.Secret);

        var identityClaim = _jwtWrapper.CreateIdentityClaim(user);

        var tokenDescriptor = _jwtWrapper.CreateTokenDescriptor(identityClaim, _config, secretKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
