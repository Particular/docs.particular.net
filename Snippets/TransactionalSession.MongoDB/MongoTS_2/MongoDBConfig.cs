using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
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

    public async Task OpenDefault(IBuilder builder)
    {
        #region open-transactional-session-mongo

        using var childBuilder = builder.CreateChildBuilder();
        var session = childBuilder.Build<ITransactionalSession>();
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
