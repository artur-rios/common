namespace ArturRios.Common.WebApi;

public abstract class WebApiStartup
{
    public abstract void ConfigureServices(IServiceCollection services);
    public abstract void ConfigureApp(IApplicationBuilder app);

    public abstract void ConfigureCors(IApplicationBuilder app);
    public abstract void ConfigureSecurity(IApplicationBuilder app);

    public virtual void StartServices(IServiceProvider serviceProvider) { }
}
