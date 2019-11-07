using NServiceBus;
using NServiceBus.Transport.SQLServer;

class DisableSubscriptionCache
{
    public DisableSubscriptionCache(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-disable-subscription-cache

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var subscriptions = transport.SubscriptionSettings();

        subscriptions.DisableSubscriptionCache();

        #endregion
    }
}