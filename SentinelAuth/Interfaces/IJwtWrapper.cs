using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SentinelAuth.Config;
using SentinelAuth.Models;

namespace SentinelAuth.Interfaces;

public interface IJwtWrapper
{
    ClaimsIdentity CreateIdentityClaim(SentinelUser user);
    SecurityTokenDescriptor CreateTokenDescriptor(ClaimsIdentity identityClaim, JwtConfig config, byte[] secretKey);
}