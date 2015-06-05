namespace Snippets4.Azure.Persistence.AzureStorage
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region PersistanceWithAzure 5

            Configure.With()
                .AzureSubscriptionStorage()
                .AzureSagaPersister()
                .UseAzureTimeoutPersister();

            #endregion
        }
    }
}