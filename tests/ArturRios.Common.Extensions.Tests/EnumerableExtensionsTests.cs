using System.Collections;

namespace ArturRios.Common.Extensions.Tests;

public class EnumerableExtensionsTests
{
    public static IEnumerable<object?[]> EmptyCollections =>
        new List<object?[]>
        {
            new object?[] { null }, new object[] { Array.Empty<int>() }, new object[] { new List<int>() }
        };

    public static IEnumerable<object[]> NotEmptyCollections => new List<object[]>
    {
        new object[] { (int[]) [1, 2, 3] }, new object[] { new List<int> { 1, 2, 3 } }
    };

    [Theory]
    [MemberData(nameof(EmptyCollections))]
    public void Should_ReturnTrue(IEnumerable collection) => Assert.True(collection.IsEmpty());

    [Theory]
    [MemberData(nameof(NotEmptyCollections))]
    public void Should_ReturnFalse(IEnumerable collection) => Assert.False(collection.IsEmpty());
}
