using ArturRios.Common.Configuration.Providers.Interfaces;
using ArturRios.Common.Extensions;

namespace ArturRios.Common.Configuration.Providers;

public class EnvironmentProvider : IConfigurationProvider
{
    public bool? GetBool(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);

        return value.ParseToBoolOrDefault();
    }

    public int? GetInt(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);

        return value.ParseToIntOrDefault();
    }

    public string? GetString(string key) => Environment.GetEnvironmentVariable(key);

    public T? GetObject<T>(string key) where T : class
    {
        var value = Environment.GetEnvironmentVariable(key);

        return value.ParseToObjectOrDefault<T>();
    }
}
