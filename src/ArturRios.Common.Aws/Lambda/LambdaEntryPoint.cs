// ReSharper disable InconsistentNaming
// Reason: this is not a test class

using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Aws.Lambda;

public abstract class LambdaEntryPoint : IDisposable
{
    private bool disposedValue;

    protected IServiceProvider ServiceProvider { get; }

    internal IServiceCollection ServiceCollection { get; }

    protected LambdaEntryPoint()
    {
        ServiceCollection = ConfigureServices();
        ServiceCollection = MockServices(ServiceCollection);
        ServiceProvider = ServiceCollection.BuildServiceProvider();
    }

    protected void Scope(Action<IServiceProvider> action)
    {
        using var scope = ServiceProvider.CreateScope();
        action(scope.ServiceProvider);
    }

    protected T Scope<T>(Func<IServiceProvider, T> func)
    {
        using var scope = ServiceProvider.CreateScope();

        return func(scope.ServiceProvider);
    }

    protected async Task ScopeAsync(Func<IServiceProvider, Task> func)
    {
        using var scope = ServiceProvider.CreateScope();

        await func(scope.ServiceProvider);
    }

    protected async Task<T> ScopeAsync<T>(Func<IServiceProvider, Task<T>> func)
    {
        using var scope = ServiceProvider.CreateScope();

        return await func(scope.ServiceProvider);
    }

    public abstract IServiceCollection ConfigureServices();

    public virtual IServiceCollection MockServices(IServiceCollection serviceCollection) => serviceCollection;

    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        if (!disposing)
        {
            return;
        }

        var serviceProvider = ServiceProvider as ServiceProvider;
        serviceProvider?.Dispose();

        foreach (var serviceDescriptor in ServiceCollection)
        {
            if (serviceDescriptor.ImplementationInstance is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        disposedValue = true;
    }

    ~LambdaEntryPoint()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
