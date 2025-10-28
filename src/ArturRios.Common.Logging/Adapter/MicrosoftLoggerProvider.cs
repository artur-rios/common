using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Logging.Adapter;

public class MicrosoftLoggerProvider(IServiceProvider serviceProvider) : ILoggerProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public ILogger CreateLogger(string categoryName) => new MicrosoftLoggerAdapter(_serviceProvider);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
