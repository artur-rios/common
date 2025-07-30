// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This class is meant to be used in other projects

// ReSharper disable MemberCanBePrivate.Global
// Reason: This class is used in other projects and it's properties and methods should be public

using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace ArturRios.Common.Util.Hashing;

public class Hash
{
    private const int Argon2IdKeyBytes = 128;
    private const int SaltByteSize = 16;

    private readonly HashConfiguration _configuration;

    private Hash(byte[] value, byte[] salt, HashConfiguration? configuration = null)
    {
        configuration ??= new HashConfiguration();

        _configuration = configuration;
        Value = value;
        Salt = salt;
    }

    private Hash(string text, byte[]? salt = null, HashConfiguration? configuration = null)
    {
        salt ??= CreateSalt();
        configuration ??= new HashConfiguration();

        _configuration = configuration;
        Value = HashText(text, salt);
        Salt = salt;
    }

    public byte[] Value { get; }
    public byte[] Salt { get; }

    public static Hash NewFromBytes(byte[] value, byte[] salt, HashConfiguration? configuration = null) =>
        new(value, salt, configuration);

    public static Hash NewFromText(string text, HashConfiguration? configuration = null, byte[]? salt = null) =>
        new(text, salt, configuration);

    private static byte[] CreateSalt()
    {
        var buffer = new byte[SaltByteSize];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(buffer);

        return buffer;
    }

    public bool TextMatches(string text)
    {
        var hashToMatch = HashText(text, Salt);

        return Value.SequenceEqual(hashToMatch);
    }

    private byte[] HashText(string text, byte[] salt)
    {
        Argon2id argon2Id = new(Encoding.UTF8.GetBytes(text))
        {
            Salt = salt,
            DegreeOfParallelism = _configuration.DegreeOfParallelism,
            Iterations = _configuration.NumberOfIterations,
            MemorySize = _configuration.MemoryToUseInKb
        };

        return argon2Id.GetBytes(Argon2IdKeyBytes);
    }
}
