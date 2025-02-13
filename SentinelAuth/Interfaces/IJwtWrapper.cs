using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SentinelAuth.Config;
using SentinelAuth.Models;

namespace SentinelAuth.Interfaces;

/// <summary>
/// Interface for a wrapper around the JWT generation library.
/// </summary>
public interface IJwtWrapper
{
    /// <summary>
    /// Create a ClaimsIdentity object containing the user's username, role and id.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    ClaimsIdentity CreateIdentityClaim(SentinelUser user);

    /// <summary>
    /// Create a SecurityTokenDescriptor object containing the ClaimsIdentity, JwtConfig and secret key.
    /// </summary>
    /// <param name="identityClaim"></param>
    /// <param name="config"></param>
    /// <param name="secretKey"></param>
    /// <returns></returns>
    SecurityTokenDescriptor CreateTokenDescriptor(ClaimsIdentity identityClaim, JwtConfig config, byte[] secretKey);
}