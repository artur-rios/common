using System.Security.Cryptography;
using System.Text;
using ArturRios.Common.Util.Collections;

namespace ArturRios.Common.Util.Random;

public static class CustomRandom
{
    public static int NumberFromRng(int start, int end, int? differentFrom = null)
    {
        end++;

        var random = RandomNumberGenerator.GetInt32(start, end);

        if (differentFrom is null)
        {
            return random;
        }

        while (random == differentFrom)
        {
            random = RandomNumberGenerator.GetInt32(start, end);
        }

        return random;
    }


    public static int NumberFromSystemRandom(int start, int end, int? differentFrom = null)
    {
        System.Random rng = new();

        var random = rng.Next(start, end);

        if (differentFrom is null)
        {
            return random;
        }

        while (random == differentFrom)
        {
            random = rng.Next(start, end);
        }

        return random;
    }

    public static string Text(RandomStringOptions options, string[]? differentFrom = null)
    {
        while (true)
        {
            var random = new System.Random();
            var password = new StringBuilder();

            var charCollectionCount = 0;

            if (options.IncludeLowercase)
            {
                password.Append(CharacterCollection.LowerChars[random.Next(CharacterCollection.LowerChars.Length)]);
                charCollectionCount++;
            }

            if (options.IncludeUppercase)
            {
                password.Append(CharacterCollection.UpperChars[random.Next(CharacterCollection.UpperChars.Length)]);
                charCollectionCount++;
            }

            if (options.IncludeSpecialCharacters)
            {
                password.Append(CharacterCollection.SpecialChars[random.Next(CharacterCollection.SpecialChars.Length)]);
                charCollectionCount++;
            }

            if (options.IncludeDigits)
            {
                password.Append(CharacterCollection.Digits[random.Next(CharacterCollection.Digits.Length)]);
                charCollectionCount++;
            }

            for (var i = charCollectionCount; i < options.Length; i++)
            {
                password.Append(CharacterCollection.AllChars[random.Next(CharacterCollection.AllChars.Length)]);
            }

            var generatedString = new string(password.ToString().OrderBy(_ => random.Next()).ToArray());
            var matchesExcludedString = false;

            if (differentFrom != null)
            {
                matchesExcludedString = differentFrom.Any(excludedString => excludedString.Equals(generatedString));
            }

            if (matchesExcludedString)
            {
                continue;
            }

            return generatedString;
        }
    }
}
