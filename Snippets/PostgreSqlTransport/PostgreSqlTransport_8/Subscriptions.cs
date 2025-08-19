using System;
using NServiceBus;
using NServiceBus.Transport.PostgreSql;

class Subscriptions
{
    void DisableSubscriptionCache(EndpointConfiguration endpointConfiguration)
    {
        #region disable-subscription-cache

        var transport = new PostgreSqlTransport("connectionString")
        {
            Subscriptions =
            {
                DisableCaching = true
            }
        };

        #endregion
    }

    void ConfigureSubscriptionCache(EndpointConfiguration endpointConfiguration)
    {
        #region configure-subscription-cache

        var transport = new PostgreSqlTransport("connectionString")
        {
            Subscriptions =
            {
                CacheInvalidationPeriod = TimeSpan.FromMinutes(1)
            }
        };

        #endregion
    }

    void ConfigureSubscriptionTable(EndpointConfiguration endpointConfiguration)
    {
        #region configure-subscription-table

        var transport = new PostgreSqlTransport("connectionString")
        {
            Subscriptions = 
            {
                SubscriptionTableName = new SubscriptionTableName(
                    table: "Subscriptions", 
                    schema: "OptionalSchema")
            }
        };

        #endregion
    }
}