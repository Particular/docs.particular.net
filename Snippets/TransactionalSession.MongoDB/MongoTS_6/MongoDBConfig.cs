using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System;
using System.Threading.Tasks;

public class MongoDBConfig
{
    public void Configure(EndpointConfiguration config)
    {
        #region enabling-transactional-session-mongo

        var persistence = config.UsePersistence<MongoPersistence>();
        persistence.EnableTransactionalSession();

        #endregion
    }

    public async Task OpenDefault(IServiceProvider serviceProvider)
    {
        #region open-transactional-session-mongo

        using var childScope = serviceProvider.CreateScope();
        var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
        await session.Open();

        // use the session

        await session.Commit();

        #endregion
    }

    public async Task UseSession(ITransactionalSession session)
    {
        #region use-transactional-session-mongo
        await session.Open();

        // add messages to the transaction:
        await session.Send(new MyMessage());

        // access the database:
        var mongoSession = session.SynchronizedStorageSession.MongoPersistenceSession();

        await session.Commit();
        #endregion
    }
}
