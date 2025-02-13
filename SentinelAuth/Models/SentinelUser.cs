namespace SentinelAuth.Models;

/// <summary>
/// Model for a user that can be authenticated by the SentinelAuth library.
/// </summary>
public class SentinelUser
{
    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Username of the user. Used as subject for the JWT.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Role of the user. Used as a claim for the JWT.
    /// </summary>
    public required string Role { get; set; }
}