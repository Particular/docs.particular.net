using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.SagaPersisters;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region PersistanceWithAzure

        endpointConfiguration.UsePersistence<AzureStoragePersistence>();

        #endregion
    }
    //TODO: fix when we split azure
    /**
    #region PersistenceWithAzureHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        }
    }

    #endregion
**/

    void CustomizingAzurePersistenceAllConnections(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSubscriptionsAllConnectionsCustomization

        endpointConfiguration.UsePersistence<AzureStoragePersistence>()
            .ConnectionString("connectionString");
        #endregion
    }

    void CustomizingAzurePersistenceSubscriptions(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSubscriptionsCustomization

        endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
            .ConnectionString("connectionString")
            .TableName("tableName")
            .CreateSchema(true);
        #endregion
    }

    void CustomizingAzurePersistenceSagas(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSagasCustomization

        endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>()
            .ConnectionString("connectionString")
            .CreateSchema(true);
        #endregion
    }

    void AzurePersistenceTimeoutsCustomization(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceTimeoutsCustomization

        endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Timeouts>()
            .ConnectionString("connectionString")
            .CreateSchema(true)
            .TimeoutManagerDataTableName("TimeoutManager")
            .TimeoutDataTableName("TimeoutData")
            .CatchUpInterval(3600)
            .PartitionKeyScope("yyyy-MM-dd-HH");
        #endregion
    }
}
