using System;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class ConfigureSubscriptionCache
{
    public ConfigureSubscriptionCache(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-configure-subscription-cache

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var subscriptions = transport.SubscriptionSettings();
        subscriptions.CacheSubscriptionInformationFor(TimeSpan.FromMinutes(1));

        #endregion
    }
}