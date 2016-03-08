namespace Snippets6.Azure.Persistence.AzureStorage
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.SagaPersisters;

    class Usage
    {
        public void Demo()
        {
            #region PersistanceWithAzure 7

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
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

        public void CustomizingAzurePersistenceAllConnections_7()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region AzurePersistenceSubscriptionsAllConnectionsCustomization 7

            endpointConfiguration.UsePersistence<AzureStoragePersistence>()
                .ConnectionString("connectionString");
            #endregion
        }

        public void CustomizingAzurePersistenceSubscriptions_7()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region AzurePersistenceSubscriptionsCustomization 7

            endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
                .ConnectionString("connectionString")
                .TableName("tableName")
                .CreateSchema(true);
            #endregion
        }

        public void CustomizingAzurePersistenceSagas_7()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region AzurePersistenceSagasCustomization 7

            endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>()
                .ConnectionString("connectionString")
                .CreateSchema(true);
            #endregion
        }

        public void AzurePersistenceTimeoutsCustomization_7()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

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

