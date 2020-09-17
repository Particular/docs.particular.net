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

        #region CosmosDBDefaultContainer

        endpointConfiguration.UsePersistence<CosmosDbPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"))
            .DefaultContainer(
                containerName: "ContainerName",
                partitionKeyPath: "/partition/key/path");

        #endregion

        #region CosmosDBDefaultContainerContainerInfo

        endpointConfiguration.UsePersistence<CosmosDbPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"))
            .DefaultContainer(new ContainerInformation(
                containerName: "ContainerName",
                partitionKeyPath: new PartitionKeyPath("/partition/key/path")));

        #endregion

        #region CosmosDBOutboxCleanup

        var outbox = endpointConfiguration.EnableOutbox();
        outbox.TimeToKeepOutboxDeduplicationData(TimeSpan.FromDays(7));

        #endregion

        #region CosmosDBRegisterLogicalBehavior

        endpointConfiguration.Pipeline.Register(new RegisterMyBehavior());

        #endregion

        #region RegisterSharedTransactionalBatchForDependencyInjection

        var persistence = endpointConfiguration.UsePersistence<CosmosDbPersistence>();
        persistence.RegisterSharedTransactionalBatchForDependencyInjection();

        #endregion
    }
}

static class TempExtensions
{
    #pragma warning disable 0649
    readonly static Container container;
    #pragma warning restore 0649

    public static TransactionalBatch GetSharedTransactionalBatch(this SynchronizedStorageSession session) => container.CreateTransactionalBatch(PartitionKey.None);
}