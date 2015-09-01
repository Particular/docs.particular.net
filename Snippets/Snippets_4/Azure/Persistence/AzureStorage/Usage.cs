namespace Snippets4.Azure.Persistence.AzureStorage
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region PersistanceWithAzure 5

            Configure configure = Configure.With();
            configure.AzureSubscriptionStorage();
            configure.AzureSagaPersister();
            configure.UseAzureTimeoutPersister();

            #endregion
        }
    }
}