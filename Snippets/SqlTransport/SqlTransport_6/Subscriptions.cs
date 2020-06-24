using System;
using NServiceBus;

class Subscriptions
{
    void DisableSubscriptionCache(EndpointConfiguration endpointConfiguration)
    {
        #region disable-subscription-cache

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var subscriptions = transport.SubscriptionSettings();

        subscriptions.DisableSubscriptionCache();

        #endregion
    }

    void ConfigureSubscriptionCache(EndpointConfiguration endpointConfiguration)
    {
        #region configure-subscription-cache

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var subscriptions = transport.SubscriptionSettings();
        subscriptions.CacheSubscriptionInformationFor(TimeSpan.FromMinutes(1));

        #endregion
    }

    void ConfigureSubscriptionTable(EndpointConfiguration endpointConfiguration)
    {
        #region configure-subscription-table

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var subscriptions = transport.SubscriptionSettings();

        subscriptions.SubscriptionTableName(
            tableName: "Subscriptions", 
            schemaName: "OptionalSchema",
            catalogName: "OptionalCatalog");

        #endregion
    }
}