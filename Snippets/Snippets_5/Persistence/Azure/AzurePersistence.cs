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
}

