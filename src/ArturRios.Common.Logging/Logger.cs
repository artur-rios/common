using System.Runtime.CompilerServices;
using ArturRios.Common.Logging.Configuration;
using ArturRios.Common.Logging.Factories;
using ArturRios.Common.Logging.Interfaces;

namespace ArturRios.Common.Logging;

public class Logger : ILogger
{
    private readonly List<IInternalLogger> _loggers = [];

    public Logger(List<LoggerConfiguration> configurations)
    {
        foreach (var config in configurations)
        {
            _loggers.Add(LoggerFactory.Create(config));
        }
    }

    public void Trace(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Trace(message, filePath, methodName);
        }
    }

    public void Debug(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Debug(message, filePath, methodName);
        }
    }

    public void Info(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Info(message, filePath, methodName);
        }
    }

    public void Warn(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Warn(message, filePath, methodName);
        }
    }

    public void Error(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Error(message, filePath, methodName);
        }
    }

    public void Exception(Exception exception, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Exception(exception, filePath, methodName);
        }
    }

    public void Critical(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Critical(message, filePath, methodName);
        }
    }

    public void Fatal(string message, [CallerFilePath] string filePath = "unknown",
        [CallerMemberName] string methodName = "unknown")
    {
        foreach (var logger in _loggers)
        {
            logger.Fatal(message, filePath, methodName);
        }
    }
}
