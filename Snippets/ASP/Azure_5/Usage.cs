using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region PersistanceWithAzure

        configure.AzureSubscriptionStorage();
        configure.AzureSagaPersister();
        configure.UseAzureTimeoutPersister();

        #endregion
    }
}
