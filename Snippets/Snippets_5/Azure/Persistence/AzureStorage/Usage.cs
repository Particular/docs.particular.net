namespace Snippets5.Azure.Persistence.AzureStorage
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.SagaPersisters;

    class Usage
    {
        public void Demo()
        {
            #region PersistanceWithAzure 6

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<AzureStoragePersistence>();

            #endregion
        }

        #region PersistenceWithAzureHost 6

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(BusConfiguration busConfiguration)
            {
                busConfiguration.UsePersistence<AzureStoragePersistence>();
            }
        }

        #endregion

        public void CustomizingAzurePersistenceSubscriptions_6_2()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region AzurePersistenceSubscriptionsCustomization 6.2

            busConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
                .ConnectionString("connectionString")
                .TableName("tableName")
                .CreateSchema(true);
            #endregion
        }

        public void CustomizingAzurePersistenceSagas_6_2()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region AzurePersistenceSagasCustomization 6.2

            busConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>()
                .ConnectionString("connectionString")
                .CreateSchema(true);
            #endregion
        }

        public void AzurePersistenceTimeoutsCustomization_6_2()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region AzurePersistenceTimeoutsCustomization 6.2

            busConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Timeouts>()
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

