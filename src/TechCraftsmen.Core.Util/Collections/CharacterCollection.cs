// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// Reason: This class is used in other projects and it's properties and methods should be public

namespace TechCraftsmen.Core.Util.Collections;

public class CharacterCollection
{
    public const string Digits = "0123456789";
    public const string LowerChars = "abcdefghijklmnopqrstuvwxyz";
    public const string UpperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string SpecialChars = "!@#$%^&*()_+-=[]{}|;':\",.<>?/";
    public const string AllChars = Digits + LowerChars + UpperChars + SpecialChars;
}
