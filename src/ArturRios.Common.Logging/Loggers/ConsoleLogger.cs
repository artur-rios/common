using ArturRios.Common.Logging.Configuration;
using ArturRios.Common.Logging.Factories;
using ArturRios.Common.Logging.Interfaces;

namespace ArturRios.Common.Logging.Loggers;

public class ConsoleLogger(ConsoleLoggerConfiguration configuration) : IInternalLogger
{
    private static readonly object s_writeLock = new();

    public void Trace(string message, string filePath, string methodName)
    {
        Write(LogLevel.Trace, filePath, methodName, message);
    }

    public void Debug(string message, string filePath, string methodName)
    {
        Write(LogLevel.Debug, filePath, methodName, message);
    }

    public void Info(string message, string filePath, string methodName)
    {
        Write(LogLevel.Information, filePath, methodName, message);
    }

    public void Warn(string message, string filePath, string methodName)
    {
        Write(LogLevel.Warning, filePath, methodName, message);
    }

    public void Error(string message, string filePath, string methodName)
    {
        Write(LogLevel.Error, filePath, methodName, message);
    }

    public void Exception(Exception? exception, string filePath, string methodName)
    {
        Write(LogLevel.Exception, filePath, methodName, exception?.ToString() ?? "(null)");
    }

    public void Critical(string message, string filePath, string methodName)
    {
        Write(LogLevel.Critical, filePath, methodName, message);
    }

    public void Fatal(string message, string filePath, string methodName)
    {
        Write(LogLevel.Fatal, filePath, methodName, message);
    }

    private static string GetAnsiColorSequence(LogLevel level) => level switch
    {
        LogLevel.Trace => ConsoleAnsiColors.DarkGray,
        LogLevel.Debug => ConsoleAnsiColors.Cyan,
        LogLevel.Information => ConsoleAnsiColors.White,
        LogLevel.Warning => ConsoleAnsiColors.Yellow,
        LogLevel.Error => ConsoleAnsiColors.Red,
        LogLevel.Exception => ConsoleAnsiColors.Magenta,
        LogLevel.Critical or LogLevel.Fatal => ConsoleAnsiColors.BrightRed,
        _ => ConsoleAnsiColors.White
    };

    private void Write(LogLevel level, string filePath, string methodName, string message)
    {
        var entry = LogEntryFactory.Create(level, filePath, methodName, message);

        if (configuration.UseColors)
        {
            _ = ConsoleAnsi.EnableVirtualTerminalProcessing();

            var ansi = GetAnsiColorSequence(level);

            const string reset = "\x1b[0m";

            lock (s_writeLock)
            {
                Console.Write(ansi + entry + reset);
            }
        }
        else
        {
            lock (s_writeLock)
            {
                Console.Write(entry);
            }
        }
    }
}
