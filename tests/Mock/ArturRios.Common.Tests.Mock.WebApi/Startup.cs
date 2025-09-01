using ArturRios.Common.WebApi;
using Microsoft.OpenApi.Models;

namespace ArturRios.Common.Tests.Mock.WebApi;

public class Startup(string[] args) : WebApiStartup(args)
{
    public override void Build()
    {
        LoadConfiguration();
        ConfigureServices();
        UseSwaggerGen(swaggerGenOptions: options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Test Web API", Version = "v1" });

            options.EnableAnnotations();
        });

        BuildApp();

        AddMiddlewares([typeof(ExceptionMiddleware)]);
        ConfigureApp();
        UseSwagger();
    }

    public override void ConfigureServices()
    {
        Builder.Services.AddEndpointsApiExplorer();
        Builder.Services.AddControllers();
    }

    public override void ConfigureApp()
    {
        App.UseHttpsRedirection();
        App.MapControllers();
    }
}
