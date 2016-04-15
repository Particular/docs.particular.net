namespace Core5.Azure.Persistence.AzureStorage
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.SagaPersisters;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region PersistanceWithAzure 6

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

        void CustomizingAzurePersistenceSubscriptions_6_2(BusConfiguration busConfiguration)
        {
            #region AzurePersistenceSubscriptionsCustomization 6.2

            busConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
                .ConnectionString("connectionString")
                .TableName("tableName")
                .CreateSchema(true);
            #endregion
        }

        void CustomizingAzurePersistenceSagas_6_2(BusConfiguration busConfiguration)
        {
            #region AzurePersistenceSagasCustomization 6.2

            busConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>()
                .ConnectionString("connectionString")
                .CreateSchema(true);
            #endregion
        }

        void AzurePersistenceTimeoutsCustomization_6_2(BusConfiguration busConfiguration)
        {
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

