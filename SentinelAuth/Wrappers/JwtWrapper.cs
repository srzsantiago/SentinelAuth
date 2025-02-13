using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SentinelAuth.Config;
using SentinelAuth.Interfaces;
using SentinelAuth.Models;

namespace SentinelAuth.Wrappers;

/// <inheritdoc />
public class JwtWrapper : IJwtWrapper
{
    /// <inheritdoc />
    public ClaimsIdentity CreateIdentityClaim(SentinelUser user)
    {
        return new ClaimsIdentity(
        [
            new(JwtRegisteredClaimNames.Sub, user.Username),
            new(ClaimTypes.Role, user.Role),
            new(JwtRegisteredClaimNames.Jti, user.Id.ToString())
        ]);
    }

    /// <inheritdoc />
    public SecurityTokenDescriptor CreateTokenDescriptor(ClaimsIdentity identityClaim, JwtConfig config, byte[] secretKey)
    {
        return new SecurityTokenDescriptor
        {
            Subject = identityClaim,
            Expires = DateTime.UtcNow.AddMinutes(config.ExpiryMinutes),
            Issuer = config.Issuer,
            Audience = config.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256
            )
        };
    }
}