namespace ArturRios.Common.Util.Random;

public class RandomStringOptions
{
    public int Length { get; set; } = 10;
    public bool IncludeLowercase { get; set; } = true;
    public bool IncludeUppercase { get; set; } = true;
    public bool IncludeDigits { get; set; } = true;
    public bool IncludeSpecialCharacters { get; set; } = true;
}
