namespace Snippets6.Azure.Persistence.AzureStorage
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.SagaPersisters;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region PersistanceWithAzure 7

            endpointConfiguration.UsePersistence<AzureStoragePersistence>();

            #endregion
        }

        #region PersistenceWithAzureHost 7

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(EndpointConfiguration endpointConfiguration)
            {
                endpointConfiguration.UsePersistence<AzureStoragePersistence>();
            }
        }

        #endregion

        void CustomizingAzurePersistenceAllConnections_7(EndpointConfiguration endpointConfiguration)
        {
            #region AzurePersistenceSubscriptionsAllConnectionsCustomization 7

            endpointConfiguration.UsePersistence<AzureStoragePersistence>()
                .ConnectionString("connectionString");
            #endregion
        }

        void CustomizingAzurePersistenceSubscriptions_7(EndpointConfiguration endpointConfiguration)
        {
            #region AzurePersistenceSubscriptionsCustomization 7

            endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
                .ConnectionString("connectionString")
                .TableName("tableName")
                .CreateSchema(true);
            #endregion
        }

        void CustomizingAzurePersistenceSagas_7(EndpointConfiguration endpointConfiguration)
        {
            #region AzurePersistenceSagasCustomization 7

            endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>()
                .ConnectionString("connectionString")
                .CreateSchema(true);
            #endregion
        }

        void AzurePersistenceTimeoutsCustomization_7(EndpointConfiguration endpointConfiguration)
        {
            #region AzurePersistenceTimeoutsCustomization 7

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
}

