using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;

public class SqlPersistenceConfig
{
    public void Configure(EndpointConfiguration config)
    {
        #region enabling-transactional-session-sqlp

        var persistence = config.UsePersistence<SqlPersistence>();
        persistence.EnableTransactionalSession();

        #endregion
    }

    public async Task OpenDefault(IServiceProvider serviceProvider)
    {
        #region open-transactional-session-sqlp

        using var childScope = serviceProvider.CreateScope();
        var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
        await session.Open(new SqlPersistenceOpenSessionOptions());

        // use the session

        await session.Commit();

        #endregion
    }

    public async Task OpenMultiTenant(IServiceProvider serviceProvider)
    {
        #region open-transactional-session-sqlp-multitenant

        using var childScope = serviceProvider.CreateScope();
        var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
        await session.Open(
                new SqlPersistenceOpenSessionOptions((
                             "MyTenantIdHeader", // Name of the header configured in this endpoint to carry the tenant ID
                             "TenantA"))); // The value of the tenant ID header

        // use the session

        await session.Commit();

        #endregion
    }

    public async Task UseSession(ITransactionalSession session)
    {
        #region use-transactional-session-sqlp
        await session.Open(new SqlPersistenceOpenSessionOptions());

        // add messages to the transaction:
        await session.Send(new MyMessage());

        // access the database:
        var sqlSession = session.SynchronizedStorageSession.SqlPersistenceSession();

        await session.Commit();
        #endregion
    }
}
