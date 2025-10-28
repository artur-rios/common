using ArturRios.Common.Logging.Configuration;
using ArturRios.Common.Logging.Factories;
using ArturRios.Common.Logging.Interfaces;

namespace ArturRios.Common.Logging;

public class StateLogger : IStateLogger
{
    private readonly List<IInternalLogger> _loggers = [];

    public string? TraceId { get; set; }

    public StateLogger(List<LoggerConfiguration> configurations)
    {
        foreach (var config in configurations)
        {
            _loggers.Add(InternalLoggerFactory.Create(config));
        }
    }

    public void Trace(string message, object? state = null)
    {
        var (fp, mn) = ResolveCallerInfo(state);
        foreach (var logger in _loggers)
        {
            logger.Trace(FormatMessageWithTraceId(message), fp, mn);
        }
    }

    public void Debug(string message, object? state = null)
    {
        var (fp, mn) = ResolveCallerInfo(state);
        foreach (var logger in _loggers)
        {
            logger.Debug(FormatMessageWithTraceId(message), fp, mn);
        }
    }

    public void Info(string message, object? state = null)
    {
        var (fp, mn) = ResolveCallerInfo(state);
        foreach (var logger in _loggers)
        {
            logger.Info(FormatMessageWithTraceId(message), fp, mn);
        }
    }

    public void Warn(string message, object? state = null)
    {
        var (fp, mn) = ResolveCallerInfo(state);
        foreach (var logger in _loggers)
        {
            logger.Warn(FormatMessageWithTraceId(message), fp, mn);
        }
    }

    public void Error(string message, object? state = null)
    {
        var (fp, mn) = ResolveCallerInfo(state);
        foreach (var logger in _loggers)
        {
            logger.Error(FormatMessageWithTraceId(message), fp, mn);
        }
    }

    public void Exception(Exception exception, object? state = null)
    {
        var (fp, mn) = ResolveCallerInfo(state);
        var msg = FormatMessageWithTraceId(exception.ToString() ?? exception.Message);
        foreach (var logger in _loggers)
        {
            logger.Exception(msg, fp, mn);
        }
    }

    public void Critical(string message, object? state = null)
    {
        var (fp, mn) = ResolveCallerInfo(state);
        foreach (var logger in _loggers)
        {
            logger.Critical(FormatMessageWithTraceId(message), fp, mn);
        }
    }

    public void Fatal(string message, object? state = null)
    {
        var (fp, mn) = ResolveCallerInfo(state);
        foreach (var logger in _loggers)
        {
            logger.Fatal(FormatMessageWithTraceId(message), fp, mn);
        }
    }

    private static (string filePath, string methodName) ResolveCallerInfo(object? state)
    {
        string? filePath = null;
        string? methodName = null;

        if (state is not IEnumerable<KeyValuePair<string, object>> pairs)
        {
            return (filePath ?? "unknown", methodName ?? "unknown");
        }

        foreach (var (key, value) in pairs)
        {
            if (value is null)
            {
                continue;
            }

            if (filePath == null && (string.Equals(key, "CallerFilePath", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(key, "FilePath", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(key, "callerFilePath", StringComparison.OrdinalIgnoreCase)))
            {
                filePath = value.ToString();
            }

            if (methodName == null && (string.Equals(key, "CallerMemberName", StringComparison.OrdinalIgnoreCase) ||
                                       string.Equals(key, "MemberName", StringComparison.OrdinalIgnoreCase) ||
                                       string.Equals(key, "Method", StringComparison.OrdinalIgnoreCase) ||
                                       string.Equals(key, "callerMemberName", StringComparison.OrdinalIgnoreCase)))
            {
                methodName = value.ToString();
            }

            if (filePath != null && methodName != null) break;
        }

        return (filePath ?? "unknown", methodName ?? "unknown");
    }

    private string FormatMessageWithTraceId(string message)
    {
        return !string.IsNullOrEmpty(TraceId) ? $"[{nameof(TraceId)}] {TraceId} | {message}" : message;
    }
}
