using ArturRios.Common.Logging.Configuration;
using ArturRios.Common.Logging.Interfaces;
using ArturRios.Common.Logging.Loggers;

namespace ArturRios.Common.Logging.Factories;

public static class LoggerFactory
{
    public static ILogger Create(LoggerConfiguration loggerConfiguration)
    {
        ArgumentNullException.ThrowIfNull(loggerConfiguration);

        return loggerConfiguration switch
        {
            ConsoleLoggerConfiguration => new ConsoleLogger(),
            FileLoggerConfiguration fileConfig => new FileLogger(fileConfig),
            _ => throw new ArgumentException($"Unsupported logger configuration type: {loggerConfiguration.GetType().FullName}", nameof(loggerConfiguration))
        };
    }
}
