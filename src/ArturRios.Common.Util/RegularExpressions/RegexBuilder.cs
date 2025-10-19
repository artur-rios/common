using System.Text;
using System.Text.RegularExpressions;

namespace ArturRios.Common.Util.RegularExpressions;

public class RegexBuilder
{
    private readonly StringBuilder _patternBuilder = new();

    public static RegexBuilder New() => new();

    public RegexBuilder WithChars(char[] chars)
    {
        _patternBuilder.Append(chars);

        return this;
    }

    public Regex Build() => new(GetPattern());

    public string GetPattern() => $"[{_patternBuilder}]";
}
