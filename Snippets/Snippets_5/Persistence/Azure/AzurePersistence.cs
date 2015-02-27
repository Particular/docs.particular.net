using NServiceBus;

class AzurePersistence
{
    public void Demo()
    {
        #region PersistanceWithAzure 6

        BusConfiguration config = new BusConfiguration();
        config.UsePersistence<AzureStoragePersistence>();

        #endregion
    }

    #region PersistenceWithAzureHost 6

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<AzureStoragePersistence>();
        }
    }

    #endregion
}

