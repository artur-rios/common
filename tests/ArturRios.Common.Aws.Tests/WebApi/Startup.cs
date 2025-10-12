using ArturRios.Common.Aws.Middleware;
using ArturRios.Common.Output;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ArturRios.Common.Aws.Tests.WebApi;

public class Startup(IConfiguration configuration)
{
    public static bool EnableApiDocs => Environment.GetEnvironmentVariable("ENABLE_API_DOCS")?.ToLower() == "true";

    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        if (EnableApiDocs)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Test Web API", Version = "v1" });

                options.EnableAnnotations();

                options.AddSecurityDefinition("api-key",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey, Name = "x-api-key", In = ParameterLocation.Header
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "api-key" }
                        },
                        ["readAccess", "writeAccess"]
                    }
                });
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .Select(e => $"Parameter: {e.Key} | Error: {e.Value?.Errors.First().ErrorMessage}").ToArray();

                    var output = DataOutput<string>.New
                        .WithData(string.Empty)
                        .WithErrors(errors);

                    return new BadRequestObjectResult(output);
                };
            });

        services.AddLogging(builder =>
        {
            builder.AddLambdaLogger(new LambdaLoggerOptions { IncludeException = true, });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler("/error/500");
        app.UseStatusCodePagesWithReExecute("/error/{0}");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.UseMiddleware<GuidTraceIdentifierMiddleware>();

        if (EnableApiDocs)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Test Web API V1");
                options.RoutePrefix = "swagger";
                options.DefaultModelExpandDepth(999);
                options.DefaultModelsExpandDepth(999);
            });
        }

        app.UseEndpoints(builder =>
        {
            builder.MapControllers();
        });
    }
}
