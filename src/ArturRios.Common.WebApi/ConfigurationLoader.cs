using ArturRios.Common.Configuration;

namespace ArturRios.Common.WebApi;

public class ConfigurationLoader
{
    private readonly IConfigurationBuilder? _configBuilder;
    private readonly WebApplicationBuilder? _webAppBuilder;
    private readonly string _basePath;
    private readonly string _environmentName;
    private EnvironmentType _defaultEnvironment = EnvironmentType.Local;
    private const string DefaultEnvFileFolder = "Environments";
    private const string DefaultAppSettingsFolder = "Settings";
    private readonly bool _isWebApp;

    public ConfigurationLoader(WebApplicationBuilder webAppBuilder, string? basePath = null)
    {
        _webAppBuilder = webAppBuilder;
        _basePath = string.IsNullOrEmpty(basePath) ? AppDomain.CurrentDomain.BaseDirectory : basePath;
        _environmentName = webAppBuilder.Environment.EnvironmentName;
        _isWebApp = true;
    }

    public ConfigurationLoader(IConfigurationBuilder configBuilder, EnvironmentType environment, string? basePath = null)
    {
        _configBuilder = configBuilder;
        _basePath = string.IsNullOrEmpty(basePath) ? AppDomain.CurrentDomain.BaseDirectory : basePath;
        _environmentName = nameof(environment);
        _isWebApp = false;
    }

    public void LoadEnvironment()
    {
        var envFolder = Path.Combine(_basePath, DefaultEnvFileFolder);
        var envFile = Path.Combine(envFolder, $".env.{_environmentName}");
        var defaultEnvFile = Path.Combine(envFolder, $".env.{nameof(_defaultEnvironment).ToLower()}");

        if (File.Exists(envFile))
        {
            DotNetEnv.Env.Load(envFile);
        }
        else if (File.Exists(defaultEnvFile))
        {
            DotNetEnv.Env.Load(defaultEnvFile);
        }
    }

    public void LoadAppSettings()
    {
        var settingsFolder = Path.Combine(_basePath, DefaultAppSettingsFolder);
        var envSettingsFile = Path.Combine(settingsFolder, $"appsettings.{_environmentName}.json");
        var defaultSettingsFile = Path.Combine(settingsFolder, $"appsettings.{nameof(_defaultEnvironment)}.json");

        if (_isWebApp)
        {
            if (File.Exists(envSettingsFile))
            {
                _webAppBuilder!.Configuration.AddJsonFile(envSettingsFile, optional: false, reloadOnChange: true);
            }
            else if (File.Exists(defaultSettingsFile))
            {
                _webAppBuilder!.Configuration.AddJsonFile(defaultSettingsFile, optional: false, reloadOnChange: true);
            }
        }
        else
        {
            if (File.Exists(envSettingsFile))
            {
                _configBuilder!.AddJsonFile(envSettingsFile, optional: false, reloadOnChange: true);
            }
            else if (File.Exists(defaultSettingsFile))
            {
                _configBuilder!.AddJsonFile(defaultSettingsFile, optional: false, reloadOnChange: true);
            }
        }
    }
}
