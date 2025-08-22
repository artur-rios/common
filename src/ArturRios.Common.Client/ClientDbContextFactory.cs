using ArturRios.Common.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Client;

public abstract class ClientDbContextFactory<TDbContext> : IDbContextFactory<TDbContext> where TDbContext : DbContext
{
    private string _connectionString = string.Empty;

    protected ClientDbContextFactory(EnvironmentType environment, ConnectionStringStrategy connectionStringStrategy)
    {
        switch (connectionStringStrategy)
        {
            case ConnectionStringStrategy.EnvironmentFile:
                SetConnectionStringFromEnvFile(environment.ToString());
                break;
            case ConnectionStringStrategy.AwsSecretsManager:
                SetConnectionStringFromAwsSecretsManager();
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

        if (dbContextInstance is null)
        {
            throw new ArgumentException($"Could not instantiate DbContext with type {typeof(TDbContext).Name}");
        }

        return dbContextInstance;
    }

    private void SetConnectionStringFromEnvFile(string environment)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var envFolder = Path.Combine(basePath, "Environments");
        var envFile = Path.Combine(envFolder, $".env.{environment.ToLower()}");

        if (!File.Exists(envFile))
        {
            throw new FileNotFoundException($".env file not found at expected location: {envFile}");
        }

        DotNetEnv.Env.Load(envFile);

        var connectionString = System.Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Database connection string is not configured in the .env file");
        }

        _connectionString = connectionString;
    }

    private void SetConnectionStringFromAwsSecretsManager()
    {
        // TODO
        throw new NotImplementedException($"Connection string strategy {ConnectionStringStrategy.AwsSecretsManager} is not implemented yet");
    }
}
