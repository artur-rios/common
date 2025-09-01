using ArturRios.Common.Configuration.Enums;
using Microsoft.Extensions.Configuration;

namespace ArturRios.Common.Configuration.Loaders;

public class ConfigurationLoader
{
    private readonly IConfigurationBuilder? _configurationBuilder;
    private readonly string _environmentName;
    private readonly string _basePath;

    public ConfigurationLoader(IConfigurationBuilder configurationBuilder, string environmentName, string? basePath = null)
    {
        _configurationBuilder = configurationBuilder;
        _environmentName = environmentName;
        _basePath = string.IsNullOrEmpty(basePath) ? AppDomain.CurrentDomain.BaseDirectory : basePath;
    }

    public ConfigurationLoader(string environmentName, string? basePath = null)
    {
        _environmentName = environmentName;
        _basePath = string.IsNullOrEmpty(basePath) ? AppDomain.CurrentDomain.BaseDirectory : basePath;
    }

    private const string DefaultEnvironmentName = nameof(EnvironmentType.Local);
    private const string DefaultEnvFileFolder = "Environments";
    private const string DefaultAppSettingsFolder = "Settings";

    public void LoadEnvironment()
    {
        var envFolder = Path.Combine(_basePath, DefaultEnvFileFolder);
        var envFile = Path.Combine(envFolder, $".env.{_environmentName}");
        var defaultEnvFile = Path.Combine(envFolder, $".env.{DefaultEnvironmentName.ToLower()}");

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
        var defaultSettingsFile = Path.Combine(settingsFolder, $"appsettings.{DefaultEnvironmentName}.json");

        if (_configurationBuilder is null)
        {
            throw new InvalidOperationException("Cannot load appsettings.json if configuration builder is not provided on constructor");
        }

        if (File.Exists(envSettingsFile))
        {
            _configurationBuilder.AddJsonFile(envSettingsFile, optional: false, reloadOnChange: true);
        }
        else if (File.Exists(defaultSettingsFile))
        {
            _configurationBuilder.AddJsonFile(defaultSettingsFile, optional: false, reloadOnChange: true);
        }
    }
}
