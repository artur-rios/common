namespace ArturRios.Common.Extensions;

public static class StringArrayExtensions
{
    public static string JoinWith(this string[] array, string separator = ", ")
    {
        return array.Length == 0 ? string.Empty : string.Join(separator, array);
    }
}
