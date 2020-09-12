using System;
using Microsoft.Azure.Cosmos;
using NServiceBus;
using NServiceBus.Persistence;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region CosmosDBUsage

        endpointConfiguration.UsePersistence<CosmosDbPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"));

        #endregion

        #region CosmosDBDatabaseName

        endpointConfiguration.UsePersistence<CosmosDbPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"))
            .DatabaseName("DatabaseName");

        #endregion

        #region CosmosDBContainer

        endpointConfiguration.UsePersistence<CosmosDbPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"))
            .Container("ContainerName","/partition/key/path");

        #endregion

        #region CosmosDBOutboxCleanup

        endpointConfiguration.UsePersistence<CosmosDbPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"))
            .TimeToKeepOutboxDeduplicationData(TimeSpan.FromDays(7));

        #endregion

        #region CosmosDBRegisterLogicalBehavior

        endpointConfiguration.Pipeline.Register(new RegisterMyBehavior());

        #endregion
    }
}

static class TempExtensions
{
    #pragma warning disable 0649
    readonly static Container container;
    #pragma warning restore 0649

    public static TransactionalBatch GetSharedTransactionalBatch(this SynchronizedStorageSession session) => container.CreateTransactionalBatch(PartitionKey.None);

    public static PersistenceExtensions<CosmosDbPersistence> TimeToKeepOutboxDeduplicationData(this PersistenceExtensions<CosmosDbPersistence> persistenceExtensions, TimeSpan timeToLive) => persistenceExtensions;
}