using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Threading.Tasks;
using NServiceBus.Persistence.AzureTable;

public class AzureStorageConfig
{
    public void Configure(EndpointConfiguration config)
    {
        #region enabling-transactional-session-azurestorage

        var persistence = config.UsePersistence<AzureTablePersistence>();
        persistence.EnableTransactionalSession();

        #endregion
    }

    public async Task OpenDefault(IBuilder builder)
    {
        #region open-transactional-session-azurestorage

        using var childBuilder = builder.CreateChildBuilder();
        var session = childBuilder.Build<ITransactionalSession>();
        await session.Open(
                new AzureTableOpenSessionOptions(
                    new TableEntityPartitionKey("MyPartitionKey")));

        // use the session

        await session.Commit();

        #endregion
    }

    public async Task OpenContainerInfo(IBuilder builder)
    {
        #region open-transactional-session-azurestorage-table

        using var childBuilder = builder.CreateChildBuilder();
        var session = childBuilder.Build<ITransactionalSession>();
        await session.Open(
                new AzureTableOpenSessionOptions(
                    new TableEntityPartitionKey("MyPartitionKey"),
                    new TableInformation("MyTable")));

        // use the session

        await session.Commit();

        #endregion
    }

    public async Task UseSession(ITransactionalSession session)
    {
        #region use-transactional-session-azurestorage

        await session.Open(
                new AzureTableOpenSessionOptions(
                    new TableEntityPartitionKey("MyPartitionKey")));

        // add messages to the transaction:
        await session.Send(new MyMessage());

        // access the database:
        var azureTableSession = session.SynchronizedStorageSession.AzureTablePersistenceSession();

        await session.Commit();

        #endregion
    }
}
