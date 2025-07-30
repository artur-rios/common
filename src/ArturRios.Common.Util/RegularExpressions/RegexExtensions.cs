using System.Text.RegularExpressions;

namespace ArturRios.Common.Util.RegularExpressions;

public static class RegexExtensions
{
    public static string Remove(this Regex regex, string @string) => regex.Replace(@string, string.Empty);
}
