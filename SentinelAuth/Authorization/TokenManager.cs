using System.IdentityModel.Tokens.Jwt;
using System.Text;
using SentinelAuth.Config;
using SentinelAuth.Interfaces;
using SentinelAuth.Models;
using SentinelAuth.Wrappers;

namespace SentinelAuth.Authorization;

/// <inheritdoc />
public class TokenManager(JwtConfig config, IJwtWrapper jwtWrapper) : ITokenManager
{
    private readonly JwtConfig _config = config;
    private readonly IJwtWrapper _jwtWrapper = jwtWrapper;

    /// <summary>
    /// Constructor that allows setting a custom JwtConfig
    /// </summary>
    /// <param name="config"></param>
    public TokenManager(JwtConfig config) : this(config, new JwtWrapper())
    {
    }

    /// <inheritdoc />
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
