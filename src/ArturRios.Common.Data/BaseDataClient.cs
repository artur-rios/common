using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Configuration.Loaders;

namespace ArturRios.Common.Data;

public abstract class BaseDataClient
{
    protected BaseDataClient(EnvironmentType environment)
    {
        var configLoader = new ConfigurationLoader(environment.ToString());
        configLoader.LoadEnvironment();
    }

    protected BaseDataClient(string connectionString)
    {
        Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", connectionString);
    }
}
