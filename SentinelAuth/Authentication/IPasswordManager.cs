namespace SentinelAuth.Authentication;

public interface IPasswordManager
{
    string CreateNewPasswordHash(string password);
    string HashPassword(string password, byte[] salt);
    bool VerifyPassword(string passwordInput, string storedPasswordString);
}