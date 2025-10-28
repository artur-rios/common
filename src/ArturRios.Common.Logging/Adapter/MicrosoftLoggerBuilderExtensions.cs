using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Logging.Adapter;

public static class MicrosoftLoggerBuilderExtensions
{
    public static ILoggingBuilder AddCustomLogger(this ILoggingBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddSingleton<ILoggerProvider, MicrosoftLoggerProvider>();

        return builder;
    }
}
