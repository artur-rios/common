// ReSharper disable MemberCanBePrivate.Global
// These properties need to be public because they are meant to be used in other projects

// ReSharper disable InconsistentNaming
// Reason: these are not test methods

using System.Text.RegularExpressions;

namespace ArturRios.Common.Util.RegularExpressions;

public static partial class RegexCollection
{
    public const string EmailPattern =
        @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

    public const string HasNumberPattern = "[0-9]+";
    public const string HasLowerCharPattern = "[a-z]+";
    public const string HasUpperCharPattern = "[A-Z]+";
    public const string HasNumberLowerAndUpperCharPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$";

    [GeneratedRegex(EmailPattern)]
    public static partial Regex Email();

    [GeneratedRegex(HasNumberPattern)]
    public static partial Regex HasNumber();

    [GeneratedRegex(HasLowerCharPattern)]
    public static partial Regex HasLowerChar();

    [GeneratedRegex(HasUpperCharPattern)]
    public static partial Regex HasUpperChar();

    [GeneratedRegex(HasNumberLowerAndUpperCharPattern)]
    public static partial Regex HasNumberLowerAndUpperChar();
}
