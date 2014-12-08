using NServiceBus;

class AzurePersistence
{
    public void Demo()
    {
        #region PersistanceWithAzure

        var config = new BusConfiguration();
        config.UsePersistence<AzureStoragePersistence>();

        #endregion
    }

    #region PersistenceWithAzureHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<AzureStoragePersistence>();
        }
    }

    #endregion

    public void CustomizingAzurePersistenceSubscriptions_6_2()
    {
        var configuration = new BusConfiguration();

        #region AzurePersistenceSubscriptionsCustomization 6.2

        configuration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
                        .ConnectionString("connectionString")
                        .TableName("tableName")
                        .CreateSchema(true);
        #endregion
    }
}

