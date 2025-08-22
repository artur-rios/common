using ArturRios.Common.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ArturRios.Common.Client;

public abstract class DataClient<TDbContext> where TDbContext : DbContext
{
    private readonly DbContext _dbContext;

    protected DataClient(EnvironmentType environment, ConnectionStringStrategy connectionStringStrategy)
    {
        _dbContext = GetDbContext(connectionStringStrategy);
    }

    private DbContext GetDbContext(ConnectionStringStrategy connectionStringStrategy)
    {
        var factoryInstance = Activator.CreateInstance(typeof(ClientDbContextFactory<>).MakeGenericType(typeof(TDbContext)), connectionStringStrategy);

        if (factoryInstance is null)
        {
            throw new ArgumentException($"Cannot instantiate DbContextFactory with type {typeof(TDbContext).Name}");
        }

        var dbContextFactory = (IDbContextFactory<TDbContext>)factoryInstance;

        return dbContextFactory.CreateDbContext();
    }
}
