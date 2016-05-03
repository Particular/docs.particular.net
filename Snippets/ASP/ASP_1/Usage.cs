using NServiceBus;
using NServiceBus.Persistence;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region PersistanceWithAzure

        endpointConfiguration.UsePersistence<AzureStoragePersistence>();

        #endregion
    }

    #region PersistenceWithAzureHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        }
    }

    #endregion

    void CustomizingAzurePersistenceAllConnections(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSubscriptionsAllConnectionsCustomization

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

        #endregion
    }

    void CustomizingAzurePersistenceSagas(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSagasCustomization

        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>();
        persistence.ConnectionString("connectionString");
        persistence.CreateSchema(true);

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