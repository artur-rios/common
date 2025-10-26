using System.Runtime.CompilerServices;
using ArturRios.Common.Logging.Factories;
using ArturRios.Common.Logging.Interfaces;

namespace ArturRios.Common.Logging.Loggers;

public class ConsoleLogger : ILogger
{
    private static readonly object s_writeLock = new();

    public void Trace(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        Write(LogLevel.Trace, filePath, methodName, message);
    }

    public void Debug(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        Write(LogLevel.Debug, filePath, methodName, message);
    }

    public void Info(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        Write(LogLevel.Information, filePath, methodName, message);
    }

    public void Warn(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        Write(LogLevel.Warning, filePath, methodName, message);
    }

    public void Error(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        Write(LogLevel.Error, filePath, methodName, message);
    }

    public void Exception(Exception? exception, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        Write(LogLevel.Exception, filePath, methodName, exception?.ToString() ?? "(null)");
    }

    public void Critical(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        Write(LogLevel.Critical, filePath, methodName, message);
    }

    public void Fatal(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        Write(LogLevel.Fatal, filePath, methodName, message);
    }

    private static string GetAnsiColorSequence(LogLevel level) => level switch
    {
        LogLevel.Trace => "\x1b[90m", // bright black / dark gray
        LogLevel.Debug => "\x1b[36m", // cyan
        LogLevel.Information => "\x1b[37m", // white
        LogLevel.Warning => "\x1b[33m", // yellow
        LogLevel.Error => "\x1b[31m", // red
        LogLevel.Exception => "\x1b[35m", // magenta
        LogLevel.Critical or LogLevel.Fatal => "\x1b[31;1m", // bright red
        _ => "\x1b[37m"
    };

    private static void Write(LogLevel level, string filePath, string methodName, string message)
    {
        var entry = LogEntryFactory.Create(level, filePath, methodName, message);

        var ansi = GetAnsiColorSequence(level);

        const string reset = "\x1b[0m";

        lock (s_writeLock)
        {
            Console.Write(ansi + entry + reset);
        }
    }
}
