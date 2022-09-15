using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace TransactionalSession_2
{
    public class AzureStorageConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-azurestorage

            config.UsePersistence<AzureTablePersistence>().EnableTransactionalSession();

            #endregion
        }

        private static async Task OpenDefault(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-azurestorage

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(new AzureTableOpenSessionOptions(new TableEntityPartitionKey("ABC")));

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }

        public async Task OpenContainerInfo(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-azurestorage-table

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(new AzureTableOpenSessionOptions(
                new TableEntityPartitionKey("ABC"),
                new TableInformation("MyTable")));

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}