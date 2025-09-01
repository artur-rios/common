using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Configuration.Loaders;
using ArturRios.Common.Configuration.Providers;
using ArturRios.Common.Extensions;
using ArturRios.Common.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
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

    private readonly Action<SwaggerGenOptions> _swaggerGenJwtAuthentication = setup =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description =
                "After getting a token from the Authentication route, put **_ONLY_** your JWT Bearer token on textbox below",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme, Type = ReferenceType.SecurityScheme
            }
        };

        setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

        setup.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
    };

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

        SetSwaggerConfigFromParameters();

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
            useSwagger = _settings.GetBool(AppSettingsKeys.SwaggerEnabled) ?? false;
        }

        if (!useSwagger)
        {
            return;
        }

        App.UseSwagger();
        App.UseSwaggerUI();
    }

    public void UseSwaggerGen(EnvironmentType[]? allowedEnvironments = null,
        Action<SwaggerGenOptions>? swaggerGenOptions = null, bool jwtAuthentication = false)
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

        Builder.Services.AddSwaggerGen(options =>
        {
            swaggerGenOptions?.Invoke(options);

            if (jwtAuthentication)
            {
                _swaggerGenJwtAuthentication.Invoke(options);
            }
        });
    }

    private void SetSwaggerConfigFromParameters()
    {
        var currentEnv = Builder.Environment.EnvironmentName;

        if (!Parameters.SwaggerEnvironments.Contains(currentEnv, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        var configValues = new Dictionary<string, string?> { [AppSettingsKeys.SwaggerEnabled] = "true" };

        Builder.Configuration.AddInMemoryCollection(configValues);
    }
}
