using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region PersistenceWithAzure

        configure.AzureSubscriptionStorage();
        configure.AzureSagaPersister();
        configure.UseAzureTimeoutPersister();

        #endregion
    }
}
