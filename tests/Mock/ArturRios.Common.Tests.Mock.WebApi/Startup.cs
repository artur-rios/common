using ArturRios.Common.Logging;
using ArturRios.Common.Logging.Configuration;
using ArturRios.Common.Logging.Interfaces;
using ArturRios.Common.Web.Api.Configuration;
using ArturRios.Common.Web.Handlers;
using ArturRios.Common.Web.Middleware;
using Microsoft.OpenApi.Models;

namespace ArturRios.Common.Tests.Mock.WebApi;

public class Startup(string[] args) : WebApiStartup(args)
{
    public override void Build()
    {
        var contentRoot = Builder.Environment.ContentRootPath;
        var logPath = Path.Combine(contentRoot, "log");

        var consoleLoggerConfig = new ConsoleLoggerConfiguration
        {
            UseColors = true
        };

        var fileLoggerConfig = new FileLoggerConfiguration
        {
            ApplicationName = "test-web-api",
            FilePath = logPath
        };

        AddCustomLogging([consoleLoggerConfig, fileLoggerConfig]);

        LoadConfiguration();

        Builder.Services.AddHttpContextAccessor();
        Builder.Services.AddTransient<TracePropagationHandler>();
        Builder.Services.AddHttpClient("external")
            .AddHttpMessageHandler<TracePropagationHandler>();

        ConfigureWebApi();
        AddDependencies();
        UseSwaggerGen(swaggerGenOptions: options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Test Web API", Version = "v1" });

            options.EnableAnnotations();
        });

        BuildApp();

        AddMiddlewares([typeof(TraceActivityMiddleware), typeof(ExceptionMiddleware)]);

        ConfigureApp();
        UseSwagger();
    }

    public override void ConfigureWebApi()
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
