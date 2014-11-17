namespace Snippets_5.Persistence.Azure
{
    using NServiceBus;

    class AzurePersistence
    {
        public void Demo()
        {
            #region PersistanceWithAzure-V5

            var config = new BusConfiguration();
            config.UsePersistence<AzureStoragePersistence>();

            #endregion
        }
    }

    #region PersistenceWithAzureHost-V5

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<AzureStoragePersistence>();
        }
    }

    #endregion
}
