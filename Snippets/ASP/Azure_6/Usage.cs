using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.SagaPersisters;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region PersistenceWithAzure

        busConfiguration.UsePersistence<AzureStoragePersistence>();

        #endregion
    }

    #region PersistenceWithAzureHost

    public class EndpointConfig :
        IConfigureThisEndpoint
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

        var persistence = busConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>();
        persistence.ConnectionString("connectionString");
        persistence.TableName("tableName");
        persistence.CreateSchema(true);

        #endregion
    }

    void CustomizingAzurePersistenceSagas_6_2(BusConfiguration busConfiguration)
    {
        #region AzurePersistenceSagasCustomization 6.2

        var persistence = busConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>();
        persistence.ConnectionString("connectionString");
        persistence.CreateSchema(true);

        #endregion
    }

    void AzurePersistenceTimeoutsCustomization_6_2(BusConfiguration busConfiguration)
    {
        #region AzurePersistenceTimeoutsCustomization 6.2

        var persistence = busConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Timeouts>();
        persistence.ConnectionString("connectionString");
        persistence.CreateSchema(true);
        persistence.TimeoutManagerDataTableName("TimeoutManager");
        persistence.TimeoutDataTableName("TimeoutData");
        persistence.CatchUpInterval(3600);
        persistence.PartitionKeyScope("yyyy-MM-dd-HH");

        #endregion
    }
}