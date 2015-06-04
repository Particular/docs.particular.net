namespace Snippets4.Persistence.Azure
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