using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
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

        var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
        var sagas = persistence.Sagas();
        sagas.EnableMigrationMode();

        #endregion

        #region CosmosDBConfigureThrottlingWithClientOptions

        endpointConfiguration.UsePersistence<CosmosPersistence>()
            .CosmosClient(new CosmosClient("ConnectionString", new CosmosClientOptions
            {
                MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(30),
                MaxRetryAttemptsOnRateLimitedRequests = 9
            }));

        #endregion

        #region CosmosDBConfigureThrottlingWithBuilder

        var cosmosClientBuilder = new CosmosClientBuilder("ConnectionString")
           .WithThrottlingRetryOptions(
               maxRetryWaitTimeOnThrottledRequests: TimeSpan.FromSeconds(30),
               maxRetryAttemptsOnThrottledRequests: 9
           );

        endpointConfiguration.UsePersistence<CosmosPersistence>()
            .CosmosClient(cosmosClientBuilder.Build());

        #endregion

        #region UsePessimisticLocking

        var sagaPersistenceConfiguration = persistence.Sagas();
        sagaPersistenceConfiguration.UsePessimisticLocking();

        #endregion

        #region PessimisticLeaseLockDuration

        var pessimisticLockingConfiguration = sagaPersistenceConfiguration.UsePessimisticLocking();
        pessimisticLockingConfiguration.SetLeaseLockTime(TimeSpan.FromMilliseconds(500));

        #endregion

        #region PessimisticLeaseLockAcquisitionTimeout

        pessimisticLockingConfiguration.SetLeaseLockAcquisitionTimeout(TimeSpan.FromMilliseconds(500));

        #endregion

        #region PessimisticLeaseLockAcquisitionMinMaxRefreshDelay

        pessimisticLockingConfiguration.SetLeaseLockAcquisitionMinimumRefreshDelay(TimeSpan.FromMilliseconds(50));
        pessimisticLockingConfiguration.SetLeaseLockAcquisitionMaximumRefreshDelay(TimeSpan.FromMilliseconds(100));

        #endregion
    }
}