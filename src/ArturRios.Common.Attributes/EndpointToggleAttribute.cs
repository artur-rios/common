using System.Net;
using ArturRios.Common.Configuration;
using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace ArturRios.Common.Attributes;

public class EndpointToggleAttribute : ActionFilterAttribute
{
    private readonly bool _enable;
    private readonly ConfigurationSourceType _configurationSource;
    private readonly string _key = string.Empty;
    private readonly string _disabledMessage;
    private readonly HttpStatusCode _disabledStatusCode;
    private readonly bool _returnContent;
    private readonly bool _useConfigurationFile;

    public EndpointToggleAttribute(
        bool enable = true,
        HttpStatusCode disabledStatusCode = HttpStatusCode.OK,
        bool returnContent = true,
        string disabledMessage = "This endpoint is currently disabled"
    )
    {
        _enable = enable;
        _disabledStatusCode = disabledStatusCode;
        _disabledMessage = disabledMessage;
        _returnContent = returnContent;
        _useConfigurationFile = false;
    }

    public EndpointToggleAttribute(
        ConfigurationSourceType configurationSource,
        string key = "",
        HttpStatusCode disabledStatusCode = HttpStatusCode.OK,
        bool returnContent = true,
        string disabledMessage = "This endpoint is currently disabled"
    )
    {
        _configurationSource = configurationSource;
        _key = key;
        _disabledStatusCode = disabledStatusCode;
        _disabledMessage = disabledMessage;
        _returnContent = returnContent;
        _useConfigurationFile = true;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var enabled = _useConfigurationFile ? GetToggleFromFile(context) : _enable;

        if (enabled)
        {
            return;
        }

        if (_returnContent)
        {
            context.Result = new WebApiOutput<object>(
                null,
                [_disabledMessage],
                true,
                (int)_disabledStatusCode
            ).ToObjectResult();
        }
        else
        {
            context.Result = new StatusCodeResult((int)_disabledStatusCode);
        }
    }

    private bool GetToggleFromFile(ActionExecutingContext context)
    {
        var toggleKey = string.IsNullOrWhiteSpace(_key) ? GetDefaultKey(context) : _key;
        return _configurationSource switch
        {
            ConfigurationSourceType.AppSettings => GetToggleFromAppSettings(context, toggleKey) ?? true,
            ConfigurationSourceType.EnvFile or ConfigurationSourceType.EnvironmentVariables =>
                GetToggleFromEnvironmentVariables(toggleKey) ?? true,
            _ => GetToggleFromAppSettings(context, toggleKey) ??
                 GetToggleFromEnvironmentVariables(toggleKey) ?? true
        };
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
