using NServiceBus;

class AzurePersistence
{
    public void Demo()
    {
        #region PersistanceWithAzure

        Configure.With()
            .AzureSubscriptionStorage()
            .AzureSagaPersister()
            .UseAzureTimeoutPersister();

        #endregion
    }
}