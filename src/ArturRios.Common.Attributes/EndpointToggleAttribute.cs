using System.Net;
using ArturRios.Common.Configuration;
using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace ArturRios.Common.Attributes;

public class EndpointToggleAttribute : ActionFilterAttribute
{
    public bool Enable { get; set; }
    public ConfigurationSourceType ConfigurationSource { get; set; }
    public string Key { get; set; } = string.Empty;
    public string DisabledMessage { get; set; } = "This endpoint is currently disabled";

    private readonly HttpStatusCode _disabledStatusCode;
    private readonly bool _useConfigurationFile;

    public EndpointToggleAttribute(bool enable, HttpStatusCode disabledStatusCode = HttpStatusCode.OK)
    {
        Enable = enable;
        _disabledStatusCode = disabledStatusCode;
        _useConfigurationFile = false;
    }

    public EndpointToggleAttribute(ConfigurationSourceType configurationSource = ConfigurationSourceType.EnvFile,
        string key = "", HttpStatusCode disabledStatusCode = HttpStatusCode.OK)
    {
        ConfigurationSource = configurationSource;
        Key = key;
        _disabledStatusCode = disabledStatusCode;
        _useConfigurationFile = true;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        bool enabled;

        if (!_useConfigurationFile)
        {
            enabled = Enable;
        }
        else
        {
            var toggleKey = string.IsNullOrWhiteSpace(Key) ? GetDefaultKey(context) : Key;
            enabled = ConfigurationSource switch
            {
                ConfigurationSourceType.AppSettings => GetToggleFromAppSettings(context, toggleKey) ?? true,
                ConfigurationSourceType.EnvFile or ConfigurationSourceType.EnvironmentVariables =>
                    GetToggleFromEnvironmentVariables(toggleKey) ?? true,
                _ => GetToggleFromAppSettings(context, toggleKey) ??
                     GetToggleFromEnvironmentVariables(toggleKey) ?? true
            };
        }

        if (!enabled)
        {
            context.Result = new WebApiOutput<object>(
                null,
                [DisabledMessage],
                true,
                (int)_disabledStatusCode
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
