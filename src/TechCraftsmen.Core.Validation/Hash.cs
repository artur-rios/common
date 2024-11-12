using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace TechCraftsmen.Core.Validation;

public class Hash
{
    public byte[] Value { get; set; }
    public byte[] Salt { get; set; }
        
    // Number of CPU Cores x 2
    private const int DegreeOfParallelism = 16;

    // Recommended minimum value
    private const int NumberOfIterations = 4;

    // 600 MB
    private const int MemoryToUseInKb = 600000;

    public Hash(byte[] value, byte[] salt)
    {
        Value = value;
        Salt = salt;
    }

    public Hash(string text, byte[]? salt = null)
    {
        salt ??= CreateSalt();

        Value = HashText(text, salt);
        Salt = salt;
    }
        
    private static byte[] CreateSalt()
    {
        var buffer = new byte[16];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(buffer);

        return buffer;
    }

    public bool TextMatches(string text)
    {
        var hashToMatch = HashText(text, Salt);

        return Value.SequenceEqual(hashToMatch);
    }

    private static byte[] HashText(string text, byte[] salt)
    {
        Argon2id argon2Id = new(Encoding.UTF8.GetBytes(text))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = NumberOfIterations,
            MemorySize = MemoryToUseInKb
        };
            
        return argon2Id.GetBytes(128);
    }
}
