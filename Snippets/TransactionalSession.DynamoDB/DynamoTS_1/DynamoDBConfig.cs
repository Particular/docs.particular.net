using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Threading.Tasks;

public class DynamoDBConfig
{
    public void Configure(EndpointConfiguration config)
    {
        #region enabling-transactional-session-dynamo

        var persistence = config.UsePersistence<DynamoDBPersistence>();
        persistence.EnableTransactionalSession();

        #endregion
    }

    public async Task OpenDefault(IBuilder builder)
    {
        #region open-transactional-session-dynamo

        using var childBuilder = builder.CreateChildBuilder();
        var session = childBuilder.Build<ITransactionalSession>();
        await session.Open(new DynamoOpenSessionOptions())
            .ConfigureAwait(false);

        // use the session

        await session.Commit()
            .ConfigureAwait(false);

        #endregion
    }

    public async Task UseSession(ITransactionalSession session)
    {
        #region use-transactional-session-dynamo
        await session.Open(new DynamoOpenSessionOptions())
            .ConfigureAwait(false);

        // add messages to the transaction:
        await session.Send(new MyMessage())
            .ConfigureAwait(false);

        // access the database:
        var dynamoSession = session.SynchronizedStorageSession.DynamoDBPersistenceSession();

        await session.Commit()
            .ConfigureAwait(false);
        #endregion
    }
}
