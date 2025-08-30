using ArturRios.Common.Configuration;
using ArturRios.Common.Extensions;
using ArturRios.Common.Output;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.WebApi;

public abstract class WebApiStartup(string[] args)
{
    protected WebApplication App = null!;
    protected readonly WebApplicationBuilder Builder = WebApplication.CreateBuilder(args);
    protected readonly WebApiParameters Parameters = new(args);

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

    public abstract void ConfigureServices(IServiceCollection services);
    public abstract void ConfigureApp();
    public virtual void ConfigureCors(IApplicationBuilder appBuilder) { }
    public virtual void ConfigureSecurity(IApplicationBuilder appBuilder) { }
    public virtual void StartServices(IServiceProvider serviceProvider) { }

    public void LoadConfiguration()
    {
        var configurationLoader = new ConfigurationLoader(Builder);

        if (Parameters.UseAppSettings)
        {
            configurationLoader.LoadAppSettings();
        }

        if (Parameters.UseEnvFile)
        {
            configurationLoader.LoadEnvironment();
        }
    }

    public void ConfigureCustomModelStateResponse(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
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
        var useSwagger = false;
        var currentEnv = Builder.Environment.EnvironmentName;
        var swaggerEnvs = Parameters.GetSwaggerEnvironments();

        if (allowedEnvironments.IsNotEmpty())
        {
            useSwagger = allowedEnvironments!.Any(env => env.ToString().Equals(currentEnv, StringComparison.OrdinalIgnoreCase));
        }
        else if (swaggerEnvs.IsNotEmpty())
        {
            useSwagger = swaggerEnvs.Contains(currentEnv, StringComparer.OrdinalIgnoreCase);
        }

        if (!useSwagger)
        {
            return;
        }

        App.UseSwagger();
        App.UseSwaggerUI();
    }
}
