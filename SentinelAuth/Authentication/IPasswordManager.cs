namespace SentinelAuth.Authentication;

/// <summary>
/// Interface for a PasswordManager that creates and verifies password hashes.
/// </summary>
public interface IPasswordManager
{
    /// <summary>
    /// Takes a plain text (password) and applies argon algorithm with a given salt to the password.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns>Password hash</returns>
    string HashPassword(string password, byte[] salt);

    /// <summary>
    /// Generates a signed password hash from the given input which should be stored to further use.
    /// </summary>
    /// <returns>Password hash</returns>
    string CreateNewPasswordHash(string password);

    /// <summary>
    /// Compared a (plain text) password input with a (hashed) stored password and determines if the password is correc.
    /// </summary>
    /// <returns>
    /// <b>true</b> if the passwords are equal; otherwise, <b>false</b>.
    /// </returns>
    bool VerifyPassword(string passwordInput, string storedPasswordString);
}