namespace SentinelAuth.Config;

/// <summary>
/// Configuration class for the JWT token generation.
/// </summary>
public class JwtConfig
{
    /// <summary>
    /// Secret (key) used to sign the token to make it inmutable. <c>NOTE:</c> Make sure this secret is not stored in code.
    /// </summary>
    public required string Secret { get; set; }

    /// <summary>
    /// The issuer claim identifies the principal that issued the JWT. It is typically a unique identifier for the authorization server or the entity that created the token.
    /// </summary>
    public required string Issuer { get; set; }


    /// <summary>
    ///  The audience claim identifies the recipients that the JWT is intended for. It can be one or more unique identifiers (like URLs or application IDs) that signify who is allowed to consume the token.
    /// </summary>
    public required string Audience { get; set; }

    /// <summary>
    /// Time in minutes after which the JWT expires. Default value is 60 minutes.
    /// </summary>
    public int ExpiryMinutes { get; set; } = 60;
}
