// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global
// Reason: This class is used as a configuration object for the CustomRandom class

namespace TechCraftsmen.Core.Util.Random;

public class RandomStringOptions
{
    public int Length { get; set; } = 10;
    public bool IncludeLowercase { get; set; } = true;
    public bool IncludeUppercase { get; set; } = true;
    public bool IncludeDigits { get; set; } = true;
    public bool IncludeSpecialCharacters { get; set; } = true;
}
