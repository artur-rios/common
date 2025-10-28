using ArturRios.Common.Extensions;

namespace ArturRios.Common.Logging.Factories;

public static class LogEntryFactory
{
    public static string Create(CustomLogLevel level, string filePath, string methodName, string message)
    {
        var logLevel = level.GetDescription()!;
        var className = Path.GetFileNameWithoutExtension(filePath);
        var timestamp = DateTime.UtcNow.ToString("o");

        return $"{logLevel}: {className} | {methodName} | {timestamp} | {message}{Environment.NewLine}";
    }
}
