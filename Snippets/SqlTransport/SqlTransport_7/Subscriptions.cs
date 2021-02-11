using System;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

class Subscriptions
{
    void DisableSubscriptionCache(EndpointConfiguration endpointConfiguration)
    {
        #region disable-subscription-cache

        var transport = new SqlServerTransport("connectionString");
        transport.Subscriptions.DisableCaching = true;

        #endregion
    }

    void ConfigureSubscriptionCache(EndpointConfiguration endpointConfiguration)
    {
        #region configure-subscription-cache

        var transport = new SqlServerTransport("connectionString");
        transport.Subscriptions.CacheInvalidationPeriod = TimeSpan.FromMinutes(1);

        #endregion
    }

    void ConfigureSubscriptionTable(EndpointConfiguration endpointConfiguration)
    {
        #region configure-subscription-table

        var transport = new SqlServerTransport("connectionString");
        transport.Subscriptions.SubscriptionTableName = new SubscriptionTableName(
            table: "Subscriptions", 
            schema: "OptionalSchema",
            catalog: "OptionalCatalog");

        #endregion
    }
}