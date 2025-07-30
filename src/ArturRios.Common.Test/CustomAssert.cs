// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: These assertion methods are meant to be used in other projects

using System.Collections;
using ArturRios.Common.Extensions;
using Xunit;

namespace ArturRios.Common.Test;

public static class CustomAssert
{
    public static void NullOrEmpty(IEnumerable? collection) => Assert.True(collection?.IsEmpty());

    public static void NotNullOrEmpty(IEnumerable? collection) => Assert.False(collection?.IsEmpty());

    public static void NullOrEmpty(string? @string) => Assert.True(string.IsNullOrEmpty(@string));

    public static void NotNullOrEmpty(string? @string) => Assert.False(string.IsNullOrEmpty(@string));

    public static void NullOrWhiteSpace(string? @string) => Assert.True(string.IsNullOrWhiteSpace(@string));

    public static void NotNullOrWhiteSpace(string? @string) => Assert.False(string.IsNullOrWhiteSpace(@string));
}
