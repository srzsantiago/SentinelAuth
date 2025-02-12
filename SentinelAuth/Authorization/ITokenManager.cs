using SentinelAuth.Models;

namespace SentinelAuth.Authorization
{
    public interface ITokenManager
    {
        string GenerateJwtToken(SentinelUser user);
    }
}