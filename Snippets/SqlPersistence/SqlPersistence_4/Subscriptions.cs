using System;
using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;


class Subscriptions
{
    void Connection(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceSubscriptionConnection

        var connection = @"Data Source=.\SqlExpress;Initial Catalog=subscriptions;Integrated Security=True";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();

        subscriptions.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });

        #endregion
    }

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