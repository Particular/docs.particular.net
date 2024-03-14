using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System;
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

    private async Task OpenDefault(IServiceProvider serviceProvider)
    {
        #region open-transactional-session-cosmos

        using var childScope = serviceProvider.CreateScope();
        var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
        await session.Open(
                new CosmosOpenSessionOptions(
                    new PartitionKey("MyPartitionKey")));

        // use the session

        await session.Commit();

        #endregion
    }

    public async Task OpenContainerInfo(IServiceProvider serviceProvider)
    {
        #region open-transactional-session-cosmos-container

        using var childScope = serviceProvider.CreateScope();
        var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
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
