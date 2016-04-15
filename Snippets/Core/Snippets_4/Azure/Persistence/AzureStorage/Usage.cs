namespace Core4.Azure.Persistence.AzureStorage
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region PersistanceWithAzure 5

            configure.AzureSubscriptionStorage();
            configure.AzureSagaPersister();
            configure.UseAzureTimeoutPersister();

            #endregion
        }
    }
}