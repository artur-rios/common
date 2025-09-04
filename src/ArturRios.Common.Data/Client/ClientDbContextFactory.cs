using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Configuration.Loaders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Data.Client;

public abstract class ClientDbContextFactory<TDbContext> : IDbContextFactory<TDbContext> where TDbContext : DbContext
{
    private string _connectionString = string.Empty;

    protected ClientDbContextFactory(EnvironmentType environment, ConnectionStringStrategy connectionStringStrategy)
    {
        switch (connectionStringStrategy)
        {
            case ConnectionStringStrategy.Environment:
                GetConnectionStringFromEnvironment(environment.ToString());
                break;
            case ConnectionStringStrategy.AwsSecretsManager:
                GetConnectionStringFromAwsSecretsManager();
                break;
            default:
                throw new ArgumentException($"Connection string strategy {connectionStringStrategy} is not supported yet");
        }
    }

    protected ClientDbContextFactory(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public TDbContext CreateDbContext()
    {
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        var dbContextOptions = new ClientDbContextOptions { ConnectionString = _connectionString };

        var dbContextInstance =
            (TDbContext)Activator.CreateInstance(typeof(TDbContext), loggerFactory, dbContextOptions)!;

        return dbContextInstance ?? throw new ArgumentException($"Could not instantiate DbContext with type {typeof(TDbContext).Name}");
    }

    private void GetConnectionStringFromEnvironment(string environment)
    {
        var configurationLoader = new ConfigurationLoader(environment);
        configurationLoader.LoadEnvironment();

        var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Database connection string is not configured in the environment variables");
        }

        _connectionString = connectionString;
    }

    private void GetConnectionStringFromAwsSecretsManager()
    {
        // TODO
        throw new NotImplementedException($"Connection string strategy {ConnectionStringStrategy.AwsSecretsManager} is not implemented yet");
    }
}
