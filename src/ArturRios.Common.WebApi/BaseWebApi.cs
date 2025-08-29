namespace ArturRios.Common.WebApi;

public abstract class BaseWebApi
{
    private const string DefaultAppSettingsFolder = "Settings";

    public WebApplicationBuilder CreateBuilder(string[] args)
    {
        return WebApplication.CreateBuilder(args);
    }

    public abstract WebApplication Build(WebApiStartup startup);

    public abstract void Run(WebApplication app);
}
