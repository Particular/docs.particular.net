using System;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Upgrade1to2
{

    void Schema(EndpointConfiguration endpointConfiguration)
    {
        #region 1to2_Schema

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.Schema("MySchema");

        #endregion
    }

    void SchemaExtended(EndpointConfiguration endpointConfiguration)
    {
        #region 1to2_Schema_Extended

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.Schema("My Schema");

        #endregion
    }

    void SubscriptionsCacheFor(EndpointConfiguration endpointConfiguration)
    {
        #region 1to2_subscriptions_CacheFor

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #endregion
    }

    void SubscriptionsDisable(EndpointConfiguration endpointConfiguration)
    {
        #region 1to2_subscriptions_Disable

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.DisableCache();

        #endregion
    }
}