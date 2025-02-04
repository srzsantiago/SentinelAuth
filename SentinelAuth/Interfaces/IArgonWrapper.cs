namespace SentinelAuth.Interfaces;

public interface IArgonWrapper
{
    byte[] HashPassword(byte[] password, byte[] salt, int memorySize, int degreeOfParallelism, int iterations, int hashSize);
    byte[] CreateSalt(int saltSize);
}

