using ArturRios.Common.Logging.Configuration;
using ArturRios.Common.Logging.Interfaces;
using ArturRios.Common.Logging.Loggers;

namespace ArturRios.Common.Logging.Factories;

public static class InternalLoggerFactory
{
    public static IInternalLogger Create(LoggerConfiguration loggerConfiguration)
    {
        ArgumentNullException.ThrowIfNull(loggerConfiguration);

        return loggerConfiguration switch
        {
            ConsoleLoggerConfiguration consoleConfig => new ConsoleLogger(consoleConfig),
            FileLoggerConfiguration fileConfig => new FileLogger(fileConfig),
            _ => throw new ArgumentException($"Unsupported logger configuration type: {loggerConfiguration.GetType().FullName}", nameof(loggerConfiguration))
        };
    }
}
