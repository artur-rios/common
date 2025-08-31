using ArturRios.Common.Configuration.Enums;
using Microsoft.Extensions.Configuration;

namespace ArturRios.Common.Configuration.Loaders;

public class ConfigurationLoader(IConfigurationBuilder configurationBuilder, string environmentName, string? basePath = null)
{
    private readonly string _basePath =
        string.IsNullOrEmpty(basePath) ? AppDomain.CurrentDomain.BaseDirectory : basePath;

    private EnvironmentType _defaultEnvironment = EnvironmentType.Local;
    private const string DefaultEnvFileFolder = "Environments";
    private const string DefaultAppSettingsFolder = "Settings";

    public void LoadEnvironment()
    {
        var envFolder = Path.Combine(_basePath, DefaultEnvFileFolder);
        var envFile = Path.Combine(envFolder, $".env.{environmentName}");
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
        var envSettingsFile = Path.Combine(settingsFolder, $"appsettings.{environmentName}.json");
        var defaultSettingsFile = Path.Combine(settingsFolder, $"appsettings.{nameof(_defaultEnvironment)}.json");

        if (File.Exists(envSettingsFile))
        {
            configurationBuilder.AddJsonFile(envSettingsFile, optional: false, reloadOnChange: true);
        }
        else if (File.Exists(defaultSettingsFile))
        {
            configurationBuilder.AddJsonFile(defaultSettingsFile, optional: false, reloadOnChange: true);
        }
    }
}
