// ReSharper disable InconsistentNaming
// Reason: these are not test methods

// ReSharper disable MemberCanBePrivate.Global
// These methods are intended to be used publicly if needed

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

    public Regex Build()
    {
        return new Regex(GetPattern());
    }
    
    public string GetPattern()
    {
        return $"[{_patternBuilder}]";
    }
}
    