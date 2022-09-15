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

        public static async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-cosmos

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new CosmosOpenSessionOptions(new PartitionKey("ABC")));

            await session.Send(new MyMessage());

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

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}