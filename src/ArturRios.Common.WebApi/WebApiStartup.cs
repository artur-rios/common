using ArturRios.Common.Configuration;
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Configuration.Loaders;
using ArturRios.Common.Configuration.Providers;
using ArturRios.Common.Extensions;
using ArturRios.Common.Output;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ArturRios.Common.WebApi;

public abstract class WebApiStartup(string[] args)
{
    protected WebApplication App = null!;
    protected readonly WebApplicationBuilder Builder = WebApplication.CreateBuilder(args);
    // ReSharper disable once MemberCanBePrivate.Global
    // Reason: this field needs to be visible if inheritor wants to access parameters
    protected readonly WebApiParameters Parameters = new(args);

    private SettingsProvider _settings = null!;

    // ReSharper disable MemberCanBeProtected.Global
    // Reason: this method needs to be public if caller wants to build only
    public abstract void Build();

    // ReSharper disable once MemberCanBePrivate.Global
    // Reason: this method needs to be public if caller wants to run only
    public void Run()
    {
        App.Run();
    }

    public void BuildAndRun()
    {
        Build();
        Run();
    }

    public void BuildApp()
    {
        App = Builder.Build();
    }

    public abstract void ConfigureServices();
    public abstract void ConfigureApp();
    public virtual void AddLogging() { }
    public virtual void ConfigureCors() { }
    public virtual void ConfigureSecurity() { }
    public virtual void StartServices() { }

    public void LoadConfiguration()
    {
        var configurationLoader = new ConfigurationLoader(Builder.Configuration, Builder.Environment.EnvironmentName);

        _settings = new SettingsProvider(Builder.Configuration);

        if (Parameters.UseAppSettings)
        {
            configurationLoader.LoadAppSettings();

            Builder.Services.AddSingleton<SettingsProvider>();
        }

        if (Parameters.UseEnvFile)
        {
            configurationLoader.LoadEnvironment();

            Builder.Services.AddSingleton<EnvironmentProvider>();
        }
    }

    public void AddMiddlewares(Type[] middlewares)
    {
        foreach (var middleware in middlewares)
        {
            if (middleware.IsSubclassOf(typeof(WebApiMiddleware)))
            {
                App.UseMiddleware(middleware);
            }
        }
    }

    public void AddCustomInvalidModelStateResponse()
    {
        Builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .Select(e => $"Parameter: {e.Key} | Error: {e.Value?.Errors.First().ErrorMessage}").ToArray();

                DataOutput<string> output = new(string.Empty, errors, false);

                return new BadRequestObjectResult(output);
            };
        });
    }

    public void UseSwagger(EnvironmentType[]? allowedEnvironments = null)
    {
        bool useSwagger;
        var currentEnv = Builder.Environment.EnvironmentName;
        var swaggerEnvs = Parameters.GetSwaggerEnvironments();

        if (allowedEnvironments.IsNotEmpty())
        {
            useSwagger = allowedEnvironments!.Any(env =>
                env.ToString().Equals(currentEnv, StringComparison.OrdinalIgnoreCase));
        }
        else if (swaggerEnvs.IsNotEmpty())
        {
            useSwagger = swaggerEnvs.Contains(currentEnv, StringComparer.OrdinalIgnoreCase);
        }
        else
        {
            useSwagger = _settings.GetBool("Swagger:Enabled") ?? false;
        }

        if (!useSwagger)
        {
            return;
        }

        App.UseSwagger();
        App.UseSwaggerUI();
    }

    public void UseSwaggerDocs(EnvironmentType[]? allowedEnvironments = null,
        Action<SwaggerGenOptions>? swaggerGenOptions = null)
    {
        var useSwaggerDocs = false;
        var currentEnv = Builder.Environment.EnvironmentName;
        var swaggerEnvs = Parameters.GetSwaggerEnvironments();

        if (allowedEnvironments.IsNotEmpty())
        {
            useSwaggerDocs = allowedEnvironments!.Any(env =>
                env.ToString().Equals(currentEnv, StringComparison.OrdinalIgnoreCase));
        }
        else if (swaggerEnvs.IsNotEmpty())
        {
            useSwaggerDocs = swaggerEnvs.Contains(currentEnv, StringComparer.OrdinalIgnoreCase);
        }

        if (!useSwaggerDocs)
        {
            return;
        }

        if (swaggerGenOptions is null)
        {
            Builder.Services.AddSwaggerGen();
        }
        else
        {
            Builder.Services.AddSwaggerGen(swaggerGenOptions);
        }
    }
}
