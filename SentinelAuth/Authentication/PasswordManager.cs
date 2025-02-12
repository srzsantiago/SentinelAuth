using System.Text;
using SentinelAuth.Config;
using SentinelAuth.Interfaces;
using SentinelAuth.Wrappers;

namespace SentinelAuth.Authentication;

public class PasswordManager(HashingConfig config, IArgonWrapper argonWrapper) : IPasswordManager
{
    private readonly HashingConfig _config = config;
    private readonly IArgonWrapper _argonWrapper = argonWrapper;

    // Default constructor (uses real implementation)
    public PasswordManager() : this(new HashingConfig(), new ArgonWrapper())
    {
    }

    // Constructor that allows setting a custom HashingConfig
    public PasswordManager(HashingConfig hashingConfig) : this(hashingConfig, new ArgonWrapper())
    {
    }

    private const string _VERSION = "1.0";
    private const string _HASH_PREFIX = $"$SENHASH$V{_VERSION}";

    /// <summary>
    /// Takes a plain text (password) and applies argon algorithm with a given salt to the password.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns>Password hash</returns>
    public string HashPassword(string password, byte[] salt)
    {
        byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);

        byte[] passwordHash = _argonWrapper.HashPassword(
            passwordInBytes,
            salt,
            _config.MemorySize,
            _config.DegreeOfParallelism,
            _config.Iterations,
            _config.HashSize
        );

        return string.Format("{0}${1}${2}", _HASH_PREFIX, Convert.ToHexString(salt), Convert.ToHexString(passwordHash));
    }

    /// <summary>
    /// Generates a signed password hash from the given input which should be stored to further use.
    /// </summary>
    /// <returns>Password hash</returns>
    public string CreateNewPasswordHash(string password)
    {
        return HashPassword(password, CreateSalt());
    }

    /// <summary>
    /// Compared a (plain text) password input with a (hashed) stored password and determines if the password is correc.
    /// </summary>
    /// <returns>
    /// <b>true</b> if the passwords are equal; otherwise, <b>false</b>.
    /// </returns>
    public bool VerifyPassword(string passwordInput, string storedPasswordString)
    {
        if (!IsHashSupported(storedPasswordString))
        {
            return false;
        }

        byte[] saltInBytes = Convert.FromHexString(GetSaltFromPasswordHash(storedPasswordString));
        string passwordInputHash = HashPassword(passwordInput, saltInBytes);
        return storedPasswordString.Equals(passwordInputHash);
    }

    /// <summary>
    /// Creates a random bytes array with the length of the configurated salt size.
    /// </summary>
    private byte[] CreateSalt()
    {
        return _argonWrapper.CreateSalt(_config.SaltSize);
    }

    /// <summary>
    /// Checks if the hash has SentinelAuth signature.
    /// </summary>
    private static bool IsHashSupported(string hashString)
    {
        return hashString.StartsWith(_HASH_PREFIX);
    }

    /// <summary>
    /// Gets the salt from a given password hash.
    /// </summary>
    private static string GetSaltFromPasswordHash(string storedHash)
    {
        string[]? splittedHashString = storedHash.Replace(_HASH_PREFIX, "").Split('$');
        return splittedHashString[1];
    }
}
