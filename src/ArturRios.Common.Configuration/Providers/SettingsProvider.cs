using ArturRios.Common.Extensions;
using Microsoft.Extensions.Configuration;
using IConfigurationProvider = ArturRios.Common.Configuration.Providers.Interfaces.IConfigurationProvider;

namespace ArturRios.Common.Configuration.Providers;

public class SettingsProvider(IConfiguration configuration) : IConfigurationProvider
{
    public bool? GetBool(string key)
    {
        var value = configuration[key];

        return value.ParseToBoolOrDefault();
    }

    public int? GetInt(string key)
    {
        var value = configuration[key];

        return value.ParseToIntOrDefault();
    }

    public string? GetString(string key) => configuration[key];

    public T? GetObject<T>(string key) where T : class
    {
        var value = configuration[key];

        return value.ParseToObjectOrDefault<T>();
    }
}
