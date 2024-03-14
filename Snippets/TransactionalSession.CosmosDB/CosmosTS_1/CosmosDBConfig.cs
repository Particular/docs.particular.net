using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

public class CosmosDBConfig
{
    public void Configure(EndpointConfiguration config)
    {
        #region enabling-transactional-session-cosmos

        var persistence = config.UsePersistence<CosmosPersistence>();
        persistence.EnableTransactionalSession();

        #endregion
    }

    public async Task OpenDefault(IBuilder builder)
    {
        #region open-transactional-session-cosmos

        using var childBuilder = builder.CreateChildBuilder();
        var session = childBuilder.Build<ITransactionalSession>();
        await session.Open(
                new CosmosOpenSessionOptions(
                    new PartitionKey("MyPartitionKey")));

        // use the session

        await session.Commit();

        #endregion
    }

    public async Task OpenContainerInfo(IBuilder builder)
    {
        #region open-transactional-session-cosmos-container

        using var childBuilder = builder.CreateChildBuilder();
        var session = childBuilder.Build<ITransactionalSession>();
        await session.Open(
                new CosmosOpenSessionOptions(
                    new PartitionKey("MyPartitionKey"),
                    new ContainerInformation(
                        "MyContainer",
                        new PartitionKeyPath("/path/to/partition/key"))));

        // use the session

        await session.Commit();

        #endregion
    }

    public async Task UseSession(ITransactionalSession session)
    {
        #region use-transactional-session-cosmos
        await session.Open(
                new CosmosOpenSessionOptions(
                    new PartitionKey("MyPartitionKey")));

        // add messages to the transaction:
        await session.Send(new MyMessage());

        // access the database:
        var cosmosSession = session.SynchronizedStorageSession.CosmosPersistenceSession();

        await session.Commit();
        #endregion
    }
}
