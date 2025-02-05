using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using SentinelAuth.Interfaces;


namespace SentinelAuth.Wrappers;

public class ArgonWrapper : IArgonWrapper
{
    public byte[] HashPassword(byte[] password, byte[] salt, int memorySize, int degreeOfParallelism, int iterations, int hashSize)
    {
        using var argon2 = new Argon2id(password)
        {
            Salt = salt,
            MemorySize = memorySize,
            DegreeOfParallelism = degreeOfParallelism,
            Iterations = iterations,
        };

        return argon2.GetBytes(hashSize);
    }

    public byte[] CreateSalt(int saltSize)
    {
        return RandomNumberGenerator.GetBytes(saltSize);
    }
}
