using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace TransactionalSession_2
{
    public class CosmosDBConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-cosmosdb

            config.UsePersistence<CosmosPersistence>().EnableTransactionalSession();

            #endregion
        }

        private static async Task OpenDefault(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-cosmosdb

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(new CosmosOpenSessionOptions(new PartitionKey("ABC")));

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }

        public async Task OpenContainerInfo(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-cosmosdb-container

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
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