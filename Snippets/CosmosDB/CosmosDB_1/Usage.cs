using System;
using Microsoft.Azure.Cosmos;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region CosmosDBUsage

        endpointConfiguration.UsePersistence<CosmosPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"));

        #endregion

        #region CosmosDBDatabaseName

        endpointConfiguration.UsePersistence<CosmosPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"))
            .DatabaseName("DatabaseName");

        #endregion

        #region CosmosDBDefaultContainer

        endpointConfiguration.UsePersistence<CosmosPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"))
            .DefaultContainer(
                containerName: "ContainerName",
                partitionKeyPath: "/partition/key/path");

        #endregion

        #region CosmosDBDisableContainerCreation

        endpointConfiguration.UsePersistence<CosmosPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString"))
            .DefaultContainer(
                containerName: "ContainerName",
                partitionKeyPath: "/partition/key/path")
            .DisableContainerCreation();

        #endregion

        #region CosmosDBOutboxCleanup

        var outbox = endpointConfiguration.EnableOutbox();
        outbox.TimeToKeepOutboxDeduplicationData(TimeSpan.FromDays(7));

        #endregion

        #region CosmosDBMigrationMode

        endpointConfiguration.UsePersistence<CosmosPersistence>()
            .EnableMigrationMode();

        #endregion

        #region CosmosDBRegisterLogicalBehavior

        endpointConfiguration.Pipeline.Register(new RegisterMyBehavior());

        #endregion
    }
}