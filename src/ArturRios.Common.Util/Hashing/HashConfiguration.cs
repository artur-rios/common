// ReSharper disable MemberCanBePrivate.Global
// Reason: This class is meant to be used in other projects, so the attributes and methods should be public

namespace ArturRios.Common.Util.Hashing;

public class HashConfiguration(
    int? degreeOfParallelism = null,
    int? numberOfIterations = null,
    int? memoryToUseInKb = null)
{
    // Number of CPU Cores x 2
    public const int DefaultDegreeOfParallelism = 16;

    // Recommended minimum value
    public const int DefaultNumberOfIterations = 4;

    // 600 MB
    public const int DefaultMemoryToUseInKb = 600000;

    public int DegreeOfParallelism { get; } = degreeOfParallelism ?? DefaultDegreeOfParallelism;

    public int NumberOfIterations { get; } = numberOfIterations ?? DefaultNumberOfIterations;

    public int MemoryToUseInKb { get; } = memoryToUseInKb ?? DefaultMemoryToUseInKb;
}
