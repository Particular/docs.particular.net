using System;
using NServiceBus;
using NServiceBus.Persistence;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region PersistenceWithAzure

        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");

        #endregion
    }

    #region PersistenceWithAzureHost

    public class EndpointConfig :
        IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence>();
            persistence.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
        }
    }

    #endregion

    void CustomizingAzurePersistenceAllConnections(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceAllConnectionsCustomization

        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString("connectionString");

        #endregion
    }

    void CustomizingAzurePersistenceSubscriptions(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSubscriptionsCustomization

        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>();
        persistence.ConnectionString("connectionString");
        persistence.TableName("tableName");
        persistence.CreateSchema(true);

        // Added in Version 1.3
        persistence.CacheFor(TimeSpan.FromMinutes(1));

        #endregion
    }

    void CustomizingAzurePersistenceSagas(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSagasCustomization

        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>();
        persistence.ConnectionString("connectionString");
        persistence.CreateSchema(true);

        // Added in Version 1.4
        persistence.AssumeSecondaryIndicesExist();

        #endregion
    }

    void AzurePersistenceTimeoutsCustomization(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceTimeoutsCustomization

        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Timeouts>();
        persistence.ConnectionString("connectionString");
        persistence.CreateSchema(true);
        persistence.TimeoutManagerDataTableName("TimeoutManager");
        persistence.TimeoutDataTableName("TimeoutData");
        persistence.CatchUpInterval(3600);
        persistence.PartitionKeyScope("yyyy-MM-dd-HH");

        #endregion
    }
}

// to avoid host reference
interface IConfigureThisEndpoint
{
}