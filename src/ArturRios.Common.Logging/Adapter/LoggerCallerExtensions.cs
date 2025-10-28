using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Logging.Adapter;

public static class LoggerCallerExtensions
{
    public static void LogTraceWithCaller(this ILogger logger, string message,
        [CallerFilePath] string callerFilePath = "unknown",
        [CallerMemberName] string callerMemberName = "unknown")
        => LogWithCaller(logger, LogLevel.Trace, message, null, callerFilePath, callerMemberName);

    public static void LogDebugWithCaller(this ILogger logger, string message,
        [CallerFilePath] string callerFilePath = "unknown",
        [CallerMemberName] string callerMemberName = "unknown")
        => LogWithCaller(logger, LogLevel.Debug, message, null, callerFilePath, callerMemberName);

    public static void LogInformationWithCaller(this ILogger logger, string message,
        [CallerFilePath] string callerFilePath = "unknown",
        [CallerMemberName] string callerMemberName = "unknown")
        => LogWithCaller(logger, LogLevel.Information, message, null, callerFilePath, callerMemberName);

    public static void LogWarningWithCaller(this ILogger logger, string message,
        [CallerFilePath] string callerFilePath = "unknown",
        [CallerMemberName] string callerMemberName = "unknown")
        => LogWithCaller(logger, LogLevel.Warning, message, null, callerFilePath, callerMemberName);

    public static void LogErrorWithCaller(this ILogger logger, string message,
        [CallerFilePath] string callerFilePath = "unknown",
        [CallerMemberName] string callerMemberName = "unknown")
        => LogWithCaller(logger, LogLevel.Error, message, null, callerFilePath, callerMemberName);

    public static void LogCriticalWithCaller(this ILogger logger, string message,
        [CallerFilePath] string callerFilePath = "unknown",
        [CallerMemberName] string callerMemberName = "unknown")
        => LogWithCaller(logger, LogLevel.Critical, message, null, callerFilePath, callerMemberName);

    public static void LogExceptionWithCaller(this ILogger logger, Exception exception, string? message = null,
        [CallerFilePath] string callerFilePath = "unknown",
        [CallerMemberName] string callerMemberName = "unknown")
        => LogWithCaller(logger, LogLevel.Error, message ?? exception.Message, exception, callerFilePath, callerMemberName);

    private static void LogWithCaller(ILogger logger, LogLevel level, string? message, Exception? exception,
        string callerFilePath, string callerMemberName)
    {
        var state = new[]
        {
            new KeyValuePair<string, object>("CallerFilePath", callerFilePath),
            new KeyValuePair<string, object>("CallerMemberName", callerMemberName),
            new KeyValuePair<string, object>("OriginalMessage", message ?? string.Empty)
        };

        logger.Log(level, new EventId(), state, exception, (s, ex) => message ?? ex?.Message ?? string.Empty);
    }
}
