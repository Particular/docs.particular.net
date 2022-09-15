using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace TransactionalSession_1
{
    public class CosmosDBConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-cosmos

            config.UsePersistence<CosmosPersistence>().EnableTransactionalSession();

            #endregion
        }

        public  async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-cosmos

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new CosmosOpenSessionOptions(new PartitionKey("ABC")));

            // use the session

            await session.Commit();

            #endregion
        }

        public async Task OpenContainerInfo(IBuilder builder)
        {
            #region open-transactional-session-cosmos-container

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new CosmosOpenSessionOptions(
                new PartitionKey("ABC"),
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
            await session.Open(new CosmosOpenSessionOptions(new PartitionKey("MyPartitionKey")));

            // add messages to the transaction:
            await session.Send(new MyMessage());

            // access the database:
            var cosmosSession = session.SynchronizedStorageSession.CosmosPersistenceSession();

            await session.Commit();
            #endregion
        }
    }
}