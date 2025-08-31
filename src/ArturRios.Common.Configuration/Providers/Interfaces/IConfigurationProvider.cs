namespace ArturRios.Common.Configuration.Providers.Interfaces;

public interface IConfigurationProvider
{
    bool? GetBool(string key);
    int? GetInt(string key);
    string? GetString(string key);
    T? GetObject<T>(string key) where T : class;
}
