using NServiceBus;

namespace Azure_5
{
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
}