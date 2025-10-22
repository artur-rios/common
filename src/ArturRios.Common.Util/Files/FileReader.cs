using System.Text.Json;

namespace ArturRios.Common.Util.Files;

public class FileReader
{
    public static string Read(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or whitespace", nameof(path));
        }

        return File.Exists(path)
            ? File.ReadAllText(path)
            : throw new FileNotFoundException($"The file at path '{path}' does not exist", path);
    }

    public static Dictionary<string, string> ReadAsDictionary(string path, char separator)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or whitespace", nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"The file at path '{path}' does not exist", path);
        }

        var lines = File.ReadAllLines(path);

        if (lines.Length < 2)
        {
            throw new InvalidOperationException("File must have at least a header and one data line");
        }

        var keys = lines[0].Split(separator);
        var dict = new Dictionary<string, string>();

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(separator);

            for (var j = 0; j < Math.Min(keys.Length, values.Length); j++)
            {
                dict[keys[j]] = values[j];
            }
        }

        return dict;
    }

    public static string[] ReadLines(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or whitespace", nameof(path));
        }

        return File.Exists(path)
            ? File.ReadAllLines(path)
            : throw new FileNotFoundException($"The file at path '{path}' does not exist", path);
    }

    public static T? ReadAndDeserialize<T>(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or whitespace", nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"The file at path '{path}' does not exist", path);
        }

        var content = File.ReadAllText(path);

        return JsonSerializer.Deserialize<T>(content);
    }
}
