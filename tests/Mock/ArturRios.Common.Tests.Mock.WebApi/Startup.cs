using ArturRios.Common.WebApi;

namespace ArturRios.Common.Tests.Mock.WebApi;

public class Startup(string[] args) : WebApiStartup(args)
{
    public override void Build()
    {
        LoadConfiguration();
        ConfigureServices(Builder.Services);

        App = Builder.Build();

        ConfigureApp();
        UseSwagger();
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
    }

    public override void ConfigureApp()
    {
        App.UseHttpsRedirection();
        App.MapControllers();
    }
}
