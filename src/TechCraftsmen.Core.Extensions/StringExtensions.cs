// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This class is meant to be used in other projects

using TechCraftsmen.Core.Util.Collections;

namespace TechCraftsmen.Core.Extensions;

public static class StringExtensions
{
    public static bool HasLowerChar(this string @string)
    {
        return RegexCollection.HasLowerChar().IsMatch(@string);
    }
    
    public static bool HasMaxLength(this string @string, int maxLength)
    {
        return !(@string.Length > maxLength);
    }
    
    public static bool HasMinLength(this string @string, int minLength)
    {
        return !(@string.Length < minLength);
    }
    
    public static bool HasNumber(this string @string)
    {
        return RegexCollection.HasNumber().IsMatch(@string);
    }

    public static bool HasUpperChar(this string @string)
    {
        return RegexCollection.HasUpperChar().IsMatch(@string);
    }
    
    public static bool IsValidEmail(this string @string)
    {
        return RegexCollection.Email().IsMatch(@string);
    }
}
