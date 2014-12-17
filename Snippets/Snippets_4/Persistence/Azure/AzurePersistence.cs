using NServiceBus;

class AzurePersistence
{
    public void Demo()
    {
        #region PersistanceWithAzure 5

        Configure.With()
            .AzureSubscriptionStorage()
            .AzureSagaPersister()
            .UseAzureTimeoutPersister();

        #endregion
    }
}