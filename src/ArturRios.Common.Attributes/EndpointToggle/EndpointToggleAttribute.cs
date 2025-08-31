using System.Net;
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Output;
using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace ArturRios.Common.Attributes.EndpointToggle;

[AttributeUsage(AttributeTargets.Method)]
public class EndpointToggleAttribute : ActionFilterAttribute
{
    private readonly bool _isEnabled;
    private readonly OutputType _disabledOutputType;
    private readonly string _disabledMessage;
    private readonly bool _useConfigurationFile;
    private readonly ConfigurationSourceType _configurationSource;
    private readonly string _keyPrefix = string.Empty;
    private readonly string _keySuffix = string.Empty;
    private readonly string _keySeparator = string.Empty;
    private readonly string _key = string.Empty;
    private readonly HttpStatusCode _disabledStatusCode;

    private ActionExecutingContext _context = null!;
    private const string DefaultAppSettingsKeyPrefix = "Endpoints:[Controller]";
    private const string DefaultEnvFileKeyPrefix = "Endpoints_[Controller]";

    public static HttpStatusCode DefaultDisabledStatusCode => HttpStatusCode.NotFound;
    public static string DefaultDisabledMessage => "This endpoint is currently disabled";

    public EndpointToggleAttribute(
        bool isEnabled = true,
        HttpStatusCode disabledStatusCode = HttpStatusCode.NotFound,
        OutputType disabledOutputType = OutputType.Object,
        string disabledMessage = "This endpoint is currently disabled"
    )
    {
        _isEnabled = isEnabled;
        _disabledStatusCode = disabledStatusCode;
        _disabledMessage = disabledMessage;
        _disabledOutputType = disabledOutputType;
        _useConfigurationFile = false;
    }

    public EndpointToggleAttribute(
        ConfigurationSourceType configurationSource,
        string key = "",
        string keyPrefix = "",
        string keySuffix = "",
        HttpStatusCode disabledStatusCode = HttpStatusCode.NotFound,
        OutputType disabledOutputType = OutputType.Object,
        string disabledMessage = "This endpoint is currently disabled"
    )
    {
        _configurationSource = configurationSource;
        _key = key;
        _disabledStatusCode = disabledStatusCode;
        _disabledMessage = disabledMessage;
        _disabledOutputType = disabledOutputType;
        _useConfigurationFile = true;
        _keySuffix = keySuffix;
        _keySeparator = configurationSource == ConfigurationSourceType.AppSettings ? ":" : "_";

        if (string.IsNullOrWhiteSpace(keyPrefix))
        {
            _keyPrefix = configurationSource == ConfigurationSourceType.AppSettings
                ? DefaultAppSettingsKeyPrefix
                : DefaultEnvFileKeyPrefix;
        }
        else
        {
            _keyPrefix = keyPrefix;
        }
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _context = context;

        var isEnabled = _useConfigurationFile ? GetToggleFromFile() : _isEnabled;

        if (isEnabled)
        {
            return;
        }

        switch (_disabledOutputType)
        {
            case OutputType.Void:
                _context.Result = new StatusCodeResult((int)_disabledStatusCode);
                break;
            case OutputType.Default:
                ReturnDefault();
                break;
            case OutputType.Object:
                ReturnObject();
                break;
            case OutputType.Exception:
                throw new EndpointDisabledException(null, DefaultDisabledMessage);
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
            _context.Result = new StatusCodeResult((int)DefaultDisabledStatusCode);

            return;
        }

        var defaultObj = returnType.IsValueType ? Activator.CreateInstance(returnType) : null;

        _context.Result = new ObjectResult(defaultObj) { StatusCode = (int)DefaultDisabledStatusCode };
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

        var keyPrefix = GetKeyPrefix(_key);

        return methodInfo is not null ? AddKeySuffix($"{keyPrefix}{_keySeparator}{methodName}") : null;
    }

    private string? GetControllerName()
    {
        var controllerActionDescriptor = _context.ActionDescriptor as ControllerActionDescriptor;
        return controllerActionDescriptor?.ControllerName;
    }

    private string GetKeyPrefix(string key)
    {
        if (string.IsNullOrWhiteSpace(_keyPrefix))
        {
            return key;
        }

        var controllerName = GetControllerName();

        return controllerName is null
            ? _keyPrefix.Replace($"{_keySeparator}[Controller]", string.Empty)
            : _keyPrefix.Replace("[Controller]", GetControllerName());
    }

    private string AddKeySuffix(string key) =>
        string.IsNullOrWhiteSpace(_keySuffix) ? key : $"{key}{_keySeparator}{_keySuffix}";
}
