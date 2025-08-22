using System.Net;
using ArturRios.Common.Configuration;
using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace ArturRios.Common.Attributes;

public class EndpointToggleAttribute(
    HttpStatusCode statusCode = HttpStatusCode.OK,
    bool? enable = null,
    ConfigurationSource? configurationSource = null,
    string? key = null)
    : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        bool enabled;

        if (enable.HasValue)
        {
            enabled = enable.Value;
        }
        else
        {
            var toggleKey = key ?? GetDefaultKey(context);

            enabled = configurationSource switch
            {
                ConfigurationSource.AppSettings => GetToggleFromAppSettings(context, toggleKey) ?? true,
                ConfigurationSource.EnvFile or ConfigurationSource.EnvironmentVariables =>
                    GetToggleFromEnvironmentVariables(toggleKey) ?? true,
                _ => GetToggleFromAppSettings(context, toggleKey) ?? GetToggleFromEnvironmentVariables(toggleKey) ?? true
            };
        }

        if (!enabled)
        {
            context.Result = new WebApiOutput<object>(
                null,
                ["Endpoint is disabled"],
                false,
                (int)statusCode
            ).ToObjectResult();
        }
    }

    private static bool? GetToggleFromAppSettings(ActionExecutingContext context, string key)
    {
        if (context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) is not IConfiguration config)
        {
            return null;
        }

        return config.GetValue<bool>(key);
    }

    private static bool? GetToggleFromEnvironmentVariables(string key)
    {
        var envValue = Environment.GetEnvironmentVariable(key);

        if (string.IsNullOrEmpty(envValue))
        {
            return null;
        }

        if (bool.TryParse(envValue, out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static string GetDefaultKey(ActionExecutingContext context)
    {
        var methodInfo = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
        var methodName = methodInfo!.Name;

        return $"{methodName}Enabled";
    }
}
