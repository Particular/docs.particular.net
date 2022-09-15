using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

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

        public static async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-azurestorage

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new AzureTableOpenSessionOptions(new TableEntityPartitionKey("ABC")));

            await session.Send(new MyMessage());

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

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}