// ReSharper disable VirtualMemberCallInConstructor
// Reason: This is intentional to allow derived classes to set up repositories during construction
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Configuration.Loaders;
using Microsoft.EntityFrameworkCore;

namespace ArturRios.Common.Data;

public abstract class BaseDataClient
{
    protected BaseDataClient(EnvironmentType environment)
    {
        var configLoader = new ConfigurationLoader(environment.ToString());
        configLoader.LoadEnvironment();

        SetRepositories();
    }

    protected BaseDataClient(string connectionString)
    {
        Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", connectionString);

        SetRepositories();
    }

    protected abstract DbContext SetRepositories();
}
