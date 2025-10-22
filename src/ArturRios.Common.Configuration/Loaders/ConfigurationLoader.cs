using ArturRios.Common.Configuration.Enums;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Configuration.Loaders;

public class ConfigurationLoader
{
    private const string DefaultEnvironmentName = nameof(EnvironmentType.Local);
    private const string DefaultEnvFileFolder = "Environments";
    private const string DefaultAppSettingsFolder = "Settings";
    private readonly string _basePath;
    private readonly IConfigurationBuilder? _configurationBuilder;
    private readonly string _environmentName;

    private readonly ILogger _logger =
        LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ConfigurationLoader>();

    public ConfigurationLoader(IConfigurationBuilder configurationBuilder, string environmentName,
        string? basePath = null)
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

    public void LoadEnvironment()
    {
        var envFolder = Path.Combine(_basePath, DefaultEnvFileFolder);
        var envFile = Path.Combine(envFolder, $".env.{_environmentName.ToLower()}");
        var defaultEnvFile = Path.Combine(envFolder, $".env.{DefaultEnvironmentName.ToLower()}");

        if (File.Exists(envFile))
        {
            _logger.LogInformation("Loading variables for {EnvironmentName} environment...", _environmentName);

            Env.Load(envFile);
        }
        else if (File.Exists(defaultEnvFile))
        {
            _logger.LogInformation(
                "Could not find variables for {EnvironmentName} environment. Loading default environment {DefaultEnvironmentName} instead...",
                _environmentName, DefaultEnvironmentName);

            Env.Load(defaultEnvFile);
        }
        else
        {
            _logger.LogInformation("Could not find any environment variables");
        }
    }

    public void LoadAppSettings()
    {
        var settingsFolder = Path.Combine(_basePath, DefaultAppSettingsFolder);
        var envSettingsFile = Path.Combine(settingsFolder, $"appsettings.{_environmentName}.json");
        var defaultSettingsFile = Path.Combine(settingsFolder, $"appsettings.{DefaultEnvironmentName}.json");

        if (_configurationBuilder is null)
        {
            throw new InvalidOperationException(
                "Cannot load appsettings.json if configuration builder is not provided on constructor");
        }

        if (File.Exists(envSettingsFile))
        {
            _logger.LogInformation("Loading app settings for {EnvironmentName} environment...", _environmentName);

            _configurationBuilder.AddJsonFile(envSettingsFile, false, true);
        }
        else if (File.Exists(defaultSettingsFile))
        {
            _logger.LogInformation(
                "Could not find app settings for {EnvironmentName} environment. Loading default environment {DefaultEnvironmentName} instead...",
                _environmentName, DefaultEnvironmentName);

            _configurationBuilder.AddJsonFile(defaultSettingsFile, false, true);
        }
        else
        {
            _logger.LogInformation("Could not find any app settings");
        }
    }
}
