// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This class is meant to be used in other projects

using System.Text.Json;
using ArturRios.Common.Util.RegularExpressions;

namespace ArturRios.Common.Extensions;

public static class StringExtensions
{
    public static bool HasLowerChar(this string @string) => RegexCollection.HasLowerChar().IsMatch(@string);

    public static bool HasMaxLength(this string @string, int maxLength) => !(@string.Length > maxLength);

    public static bool HasMinLength(this string @string, int minLength) => !(@string.Length < minLength);

    public static bool HasNumber(this string @string) => RegexCollection.HasNumber().IsMatch(@string);

    public static bool HasUpperChar(this string @string) => RegexCollection.HasUpperChar().IsMatch(@string);

    public static bool IsValidEmail(this string @string) => RegexCollection.Email().IsMatch(@string);

    public static string TrimChar(this string input, char charToTrim) =>
        string.IsNullOrEmpty(input) ? input : input.Trim().Trim(charToTrim);

    public static bool ParseToBoolOrDefault(this string? @string, bool defaultValue = false)
    {
        return bool.TryParse(@string, out var result) ? result : defaultValue;
    }

    public static int ParseToIntOrDefault(this string? @string, int defaultValue = 0)
    {
        return int.TryParse(@string, out var result) ? result : defaultValue;
    }

    public static T? ParseToObjectOrDefault<T>(this string? @string) where T : class
    {
        if (string.IsNullOrEmpty(@string))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(@string);
        }
        catch
        {
            return null;
        }
    }

    public static bool IsValidEnumValue<TEnum>(this string @string, bool ignoreCase = true) where TEnum : Enum
    {
        return Enum.TryParse(typeof(TEnum), @string, ignoreCase, out _);
    }

    public static string? ValueOrDefault(this string? @string, string? defaultValue = null)
    {
        return string.IsNullOrEmpty(@string) ? defaultValue : @string;
    }

    public static string JoinWith(this IEnumerable<string> source, string separator = ", ") =>
        string.Join(separator, source);

    public static string JoinWith<T>(this IEnumerable<T> source, string separator = ", ") =>
        string.Join(separator, source.Select(x => x?.ToString()));
}
