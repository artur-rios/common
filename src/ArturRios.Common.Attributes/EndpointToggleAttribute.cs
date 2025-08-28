using System.Net;
using ArturRios.Common.Configuration;
using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace ArturRios.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class EndpointToggleAttribute : ActionFilterAttribute
{
    private readonly bool _isEnabled;
    private readonly ReturnType _disabledReturnType;
    private readonly string _disabledMessage;
    private readonly bool _useConfigurationFile;
    private readonly ConfigurationSourceType _configurationSource;
    private readonly string _key = string.Empty;
    private readonly HttpStatusCode _disabledStatusCode;

    private ActionExecutingContext _context = null!;

    public EndpointToggleAttribute(
        bool isEnabled = true,
        HttpStatusCode disabledStatusCode = HttpStatusCode.OK,
        ReturnType disabledReturnType = ReturnType.Object,
        string disabledMessage = "This endpoint is currently disabled"
    )
    {
        _isEnabled = isEnabled;
        _disabledStatusCode = disabledStatusCode;
        _disabledMessage = disabledMessage;
        _disabledReturnType = disabledReturnType;
        _useConfigurationFile = false;
    }

    public EndpointToggleAttribute(
        ConfigurationSourceType configurationSource,
        string key = "",
        HttpStatusCode disabledStatusCode = HttpStatusCode.OK,
        ReturnType disabledReturnType = ReturnType.Object,
        string disabledMessage = "This endpoint is currently disabled"
    )
    {
        _configurationSource = configurationSource;
        _key = key;
        _disabledStatusCode = disabledStatusCode;
        _disabledMessage = disabledMessage;
        _disabledReturnType = disabledReturnType;
        _useConfigurationFile = true;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _context = context;

        var isEnabled = _useConfigurationFile ? GetToggleFromFile() : _isEnabled;

        if (isEnabled)
        {
            return;
        }

        switch (_disabledReturnType)
        {
            case ReturnType.Void:
                _context.Result = new StatusCodeResult((int)_disabledStatusCode);
                break;
            case ReturnType.Default:
                ReturnDefault();
                break;
            case ReturnType.Object:
                ReturnObject();
                break;
            case ReturnType.Exception:
                throw new InvalidOperationException("This endpoint is currently disabled");
            default:
                ReturnObject();
                break;
        }
    }

    private void ReturnObject()
    {
        _context.Result = new WebApiOutput<object>(
            null,
            [_disabledMessage],
            true,
            (int)_disabledStatusCode
        ).ToObjectResult();
    }

    private void ReturnDefault()
    {
        var methodInfo = (_context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
        var returnType = methodInfo?.ReturnType;

        if (returnType == null || returnType == typeof(void))
        {
            _context.Result = new NoContentResult();

            return;
        }

        var defaultObj = returnType.IsValueType ? Activator.CreateInstance(returnType) : null;

        _context.Result = new OkObjectResult(defaultObj);
    }

    private bool GetToggleFromFile()
    {
        var toggleKey = string.IsNullOrWhiteSpace(_key) ? GetDefaultKey() : _key;

        if (string.IsNullOrWhiteSpace(toggleKey))
        {
            return true;
        }

        return _configurationSource switch
        {
            ConfigurationSourceType.AppSettings => GetToggleFromAppSettings(toggleKey) ?? true,
            ConfigurationSourceType.EnvFile or ConfigurationSourceType.EnvironmentVariables =>
                GetToggleFromEnvironmentVariables(toggleKey) ?? true,
            _ => GetToggleFromAppSettings(toggleKey) ??
                 GetToggleFromEnvironmentVariables(toggleKey) ?? true
        };
    }

    private bool? GetToggleFromAppSettings(string key)
    {
        if (_context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) is not IConfiguration config)
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

    private string? GetDefaultKey()
    {
        var methodInfo = (_context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
        var methodName = methodInfo?.Name;

        return methodInfo is not null ? $"{methodName}Enabled" : null;
    }
}
