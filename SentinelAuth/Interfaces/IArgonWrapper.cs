namespace SentinelAuth.Interfaces;

/// <summary>
/// Interface for a wrapper around the Argon2 password hashing algorithm.
/// </summary>
public interface IArgonWrapper
{
    /// <summary>
    /// Hashes a password using the Argon2 algorithm.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <param name="memorySize"></param>
    /// <param name="degreeOfParallelism"></param>
    /// <param name="iterations"></param>
    /// <param name="hashSize"></param>
    /// <returns></returns>
    byte[] HashPassword(byte[] password, byte[] salt, int memorySize, int degreeOfParallelism, int iterations, int hashSize);

    /// <summary>
    /// Creates a random salt of the given size.
    /// </summary>
    /// <param name="saltSize"></param>
    /// <returns></returns>
    byte[] CreateSalt(int saltSize);
}

