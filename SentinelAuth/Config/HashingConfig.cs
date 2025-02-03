namespace SentinelAuth.Config;

public class HashingConfig
{
    /// <summary>
    /// The memory size used by the algorithm. The higher the better as it makes hash cracking more expensive for an attacker. Default value is 64MB (65536 kilobytes)
    /// </summary>
    public int MemorySize { get; set; } = 65536;

    /// <summary>
    /// The number of threads/cores to use. This depends on the amount of cores available. Most machines have at least 4. This should be chosen as high as possible to reduce the threat imposed by parallelized hash cracking. Default value is 4 cores
    /// </summary>
    public int DegreeOfParallelism { get; set; } = 4;

    /// <summary>
    /// The number of iterations over the memory. The execution time correlates linearly with this parameter. It allows you to increase the computational cost required to calculate one hash. Default value is 4 iterations
    /// </summary>
    public int Iterations { get; set; } = 4;

    /// <summary>
    /// The size of the salt in bytes. A random data fed as an additional input to a one-way function that hashes data, a password or passphrase. This avoid two identical passwords from generating the same hash. Default value is 16 bytes (128 bits)
    /// </summary>
    public int SaltSize { get; set; } = 16;

    /// <summary>
    /// The length of the hash in bytes. The Argon2 algorithm authors claim that a value of 128 bits should be sufficient for most applications. Default value is 32 bytes (128 bits)
    /// </summary>
    public int HashSize { get; set; } = 32;
}
