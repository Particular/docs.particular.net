using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus.Persistence.AzureTable;

namespace TransactionalSession_1
{
    public class AzureStorageConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-azurestorage

            config.UsePersistence<AzureTablePersistence>().EnableTransactionalSession();

            #endregion
        }

        public async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-azurestorage

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new AzureTableOpenSessionOptions(new TableEntityPartitionKey("ABC")));

            // use the session

            await session.Commit();

            #endregion
        }

        public async Task OpenContainerInfo(IBuilder builder)
        {
            #region open-transactional-session-azurestorage-table

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new AzureTableOpenSessionOptions(
                new TableEntityPartitionKey("ABC"),
                new TableInformation("MyTable")));

            // use the session

            await session.Commit();

            #endregion
        }

        public async Task UseSession(ITransactionalSession session)
        {
            #region use-transactional-session-azurestorage
            await session.Open(new AzureTableOpenSessionOptions(new TableEntityPartitionKey("ABC")));

            // add messages to the transaction:
            await session.Send(new MyMessage());

            // access the database:
            var azureTableSession = session.SynchronizedStorageSession.AzureTablePersistenceSession();

            await session.Commit();
            #endregion
        }
    }
}