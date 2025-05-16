// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: These assertion methods are meant to be used in other projects

using System.Collections;
using TechCraftsmen.Core.Extensions;
using Xunit;

namespace TechCraftsmen.Core.Test;

public static class CustomAssert
{
    public static void NotNullOrEmpty(IEnumerable? collection)
    {
        Assert.False(collection?.Empty());
    }
    
    public static void NotNullOrEmpty(string? @string)
    {
        Assert.False(string.IsNullOrEmpty(@string));
    }
    
    public static void NotNullOrWhiteSpace(string? @string)
    {
        Assert.False(string.IsNullOrWhiteSpace(@string));
    }
}
