using System;
using Microsoft.Azure.Cosmos;
using NServiceBus;

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
            .DefaultContainer(
                containerName: "ContainerName",
                partitionKeyPath: new PartitionKeyPath("/partition/key/path"));

        #endregion

        #region CosmosDBOutboxCleanup

        var outbox = endpointConfiguration.EnableOutbox();
        outbox.TimeToKeepOutboxDeduplicationData(TimeSpan.FromDays(7));

        #endregion

        #region CosmosDBRegisterLogicalBehavior

        endpointConfiguration.Pipeline.Register(new RegisterMyBehavior());

        #endregion
    }
}