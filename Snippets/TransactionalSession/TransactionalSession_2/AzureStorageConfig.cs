using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus.Persistence.AzureTable;

namespace TransactionalSession_2
{
    public class AzureStorageConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-azurestorage

            var persistence = config.UsePersistence<AzureTablePersistence>();
            persistence.EnableTransactionalSession();

            #endregion
        }

        private async Task OpenDefault(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-azurestorage

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(
                    new AzureTableOpenSessionOptions(
                        new TableEntityPartitionKey("MyPartitionKey")))
                .ConfigureAwait(false);

            // use the session

            await session.Commit()
                .ConfigureAwait(false);

            #endregion
        }

        public async Task OpenContainerInfo(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-azurestorage-table

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(
                    new AzureTableOpenSessionOptions(
                        new TableEntityPartitionKey("MyPartitionKey"),
                        new TableInformation("MyTable")))
                .ConfigureAwait(false);

            // use the session

            await session.Commit()
                .ConfigureAwait(false);

            #endregion
        }

        public async Task UseSession(ITransactionalSession session)
        {
            #region use-transactional-session-azurestorage
            await session.Open(
                    new AzureTableOpenSessionOptions(
                        new TableEntityPartitionKey("MyPartitionKey")))
                .ConfigureAwait(false);

            // add messages to the transaction:
            await session.Send(new MyMessage())
                .ConfigureAwait(false);

            // access the database:
            var azureTableSession = session.SynchronizedStorageSession.AzureTablePersistenceSession();

            await session.Commit()
                .ConfigureAwait(false);
            #endregion
        }
    }
}