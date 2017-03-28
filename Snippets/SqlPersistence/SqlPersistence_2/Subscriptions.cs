using System;
using NServiceBus;
using NServiceBus.Persistence.Sql;


class Subscriptions
{

    void SubscriptionsCacheFor(EndpointConfiguration endpointConfiguration)
    {
        #region subscriptions_CacheFor

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #endregion
    }

    void SubscriptionsDisable(EndpointConfiguration endpointConfiguration)
    {
        #region subscriptions_Disable

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.DisableCache();

        #endregion
    }
}