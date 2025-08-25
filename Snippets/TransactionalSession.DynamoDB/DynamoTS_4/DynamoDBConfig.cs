using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;

public class DynamoDBConfig
{
    public void Configure(EndpointConfiguration config)
    {
        #region enabling-transactional-session-dynamo

        var persistence = config.UsePersistence<DynamoPersistence>();
        persistence.EnableTransactionalSession();

        #endregion
    }

    public async Task OpenDefault(IServiceProvider serviceProvider)
    {
        #region open-transactional-session-dynamo

        using var childScope = serviceProvider.CreateScope();
        var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
        await session.Open(new DynamoOpenSessionOptions());

        // use the session

        await session.Commit();

        #endregion
    }

    public async Task UseSession(ITransactionalSession session)
    {
        #region use-transactional-session-dynamo
        await session.Open(new DynamoOpenSessionOptions());

        // add messages to the transaction:
        await session.Send(new MyMessage());

        // access the database:
        var dynamoSession = session.SynchronizedStorageSession.DynamoPersistenceSession();

        await session.Commit();
        #endregion
    }
}
