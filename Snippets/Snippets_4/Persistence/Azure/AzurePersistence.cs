namespace Snippets_4.Persistence.Azure
{
    using NServiceBus;

    class AzurePersistence
    {
        public void Demo()
        {
            #region PersistanceWithAzure-V4

            Configure.With()
                .AzureSubscriptionStorage()
                .AzureSagaPersister()
                .UseAzureTimeoutPersister();

            #endregion
        }
    }
}
