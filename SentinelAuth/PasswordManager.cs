using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using SentinelAuth.Config;

namespace SentinelAuth;

public class PasswordManager(HashingConfig config)
{
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

        using var argon2Algorithm = new Argon2id(passwordInBytes)
        {
            Salt = salt,
            MemorySize = config.MemorySize,
            DegreeOfParallelism = config.DegreeOfParallelism,
            Iterations = config.Iterations,
        };

        byte[] passwordHash = argon2Algorithm.GetBytes(config.HashSize);
        return string.Format("{0}${1}${2}", _HASH_PREFIX, Convert.ToHexString(salt), Convert.ToHexString(passwordHash));
    }

    /// <summary>
    /// Generates a signed password hash from the given input which should be stored to further use.
    /// </summary>
    /// <returns>Password hash</returns>
    public string CreateNewPassword(string input)
    {
        return HashPassword(input, CreateSalt());
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
        SplitHashString(storedPasswordString, out string salt, out string passwordHash);

        byte[] saltInBytes = Convert.FromHexString(salt);
        string passwordInputHash = HashPassword(passwordInput, saltInBytes);
        return storedPasswordString.Equals(passwordInputHash);
    }  

    /// <summary>
    /// Creates a random bytes array with the length of the configurated salt size.
    /// </summary>
    private byte[] CreateSalt()
    {
        return RandomNumberGenerator.GetBytes(config.SaltSize);
    }

    /// <summary>
    /// Checks if the hash has SentinelAuth signature.
    /// </summary>
    private static bool IsHashSupported(string hashString)
    {
        return hashString.StartsWith(_HASH_PREFIX);
    }

    /// <summary>
    /// Is given a passwordHash and split the value in the actual hash and the salt.
    /// </summary>
    private static void SplitHashString(string storedHash, out string salt, out string hash)
    {
        string[]? splittedHashString = storedHash.Replace(_HASH_PREFIX, "").Split('$');
        salt = splittedHashString[1];
        hash = splittedHashString[2];
    }
}
