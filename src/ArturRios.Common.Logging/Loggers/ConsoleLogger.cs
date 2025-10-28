using ArturRios.Common.Logging.Configuration;
using ArturRios.Common.Logging.Factories;
using ArturRios.Common.Logging.Interfaces;

namespace ArturRios.Common.Logging.Loggers;

public class ConsoleLogger(ConsoleLoggerConfiguration configuration) : IInternalLogger
{
    private static readonly object s_writeLock = new();

    public void Trace(string message, string filePath, string methodName)
    {
        Write(CustomLogLevel.Trace, filePath, methodName, message);
    }

    public void Debug(string message, string filePath, string methodName)
    {
        Write(CustomLogLevel.Debug, filePath, methodName, message);
    }

    public void Info(string message, string filePath, string methodName)
    {
        Write(CustomLogLevel.Information, filePath, methodName, message);
    }

    public void Warn(string message, string filePath, string methodName)
    {
        Write(CustomLogLevel.Warning, filePath, methodName, message);
    }

    public void Error(string message, string filePath, string methodName)
    {
        Write(CustomLogLevel.Error, filePath, methodName, message);
    }

    public void Exception(string message, string filePath, string methodName)
    {
        Write(CustomLogLevel.Exception, filePath, methodName, message);
    }

    public void Critical(string message, string filePath, string methodName)
    {
        Write(CustomLogLevel.Critical, filePath, methodName, message);
    }

    public void Fatal(string message, string filePath, string methodName)
    {
        Write(CustomLogLevel.Fatal, filePath, methodName, message);
    }

    private static string GetAnsiColorSequence(CustomLogLevel level) => level switch
    {
        CustomLogLevel.Trace => ConsoleAnsiColors.DarkGray,
        CustomLogLevel.Debug => ConsoleAnsiColors.Cyan,
        CustomLogLevel.Information => ConsoleAnsiColors.Green,
        CustomLogLevel.Warning => ConsoleAnsiColors.Yellow,
        CustomLogLevel.Error => ConsoleAnsiColors.Red,
        CustomLogLevel.Exception => ConsoleAnsiColors.Magenta,
        CustomLogLevel.Critical or CustomLogLevel.Fatal => ConsoleAnsiColors.BrightRed,
        _ => ConsoleAnsiColors.White
    };

    private void Write(CustomLogLevel level, string filePath, string methodName, string message)
    {
        var entry = LogEntryFactory.Create(level, filePath, methodName, message);

        if (configuration.UseColors)
        {
            _ = ConsoleAnsi.EnableVirtualTerminalProcessing();

            var ansiColor = GetAnsiColorSequence(level);

            const string colorReset = "\x1b[0m";

            lock (s_writeLock)
            {
                Console.Write(ansiColor + entry + colorReset);
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
