using System.Runtime.CompilerServices;
using ArturRios.Common.Logging.Configuration;
using ArturRios.Common.Logging.Factories;
using ArturRios.Common.Logging.Interfaces;

namespace ArturRios.Common.Logging;

public class StandaloneLogger : IStandaloneLogger
{
    private readonly List<IInternalLogger> _loggers = [];

    public string? TraceId { get; set; }

    public StandaloneLogger(List<LoggerConfiguration> configurations)
    {
        foreach (var config in configurations)
        {
            _loggers.Add(InternalLoggerFactory.Create(config));
        }
    }

    public void Trace(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Trace(FormatMessageWithTraceId(message), filePath, methodName);
        }
    }

    public void Debug(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Debug(FormatMessageWithTraceId(message), filePath, methodName);
        }
    }

    public void Info(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Info(FormatMessageWithTraceId(message), filePath, methodName);
        }
    }

    public void Warn(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Warn(FormatMessageWithTraceId(message), filePath, methodName);
        }
    }

    public void Error(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Error(FormatMessageWithTraceId(message), filePath, methodName);
        }
    }

    public void Exception(Exception exception, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Exception(FormatMessageWithTraceId(exception.Message), filePath, methodName);
        }
    }

    public void Critical(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Critical(FormatMessageWithTraceId(message), filePath, methodName);
        }
    }

    public void Fatal(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Fatal(FormatMessageWithTraceId(message), filePath, methodName);
        }
    }

    private string FormatMessageWithTraceId(string message)
    {
        return !string.IsNullOrEmpty(TraceId) ? $"[{nameof(TraceId)}] {TraceId} | {message}" : message;
    }
}
