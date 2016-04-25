namespace ASQ_7.UpgradeGuides._6to7
{
    using NServiceBus;

    public class Upgrade
    {
        void UseTransport(EndpointConfiguration endpointConfiguration)
        {
            #region AzureStorageQueuePurgeOnStartup

            endpointConfiguration.PurgeOnStartup(true);

            #endregion
        }
    }
}