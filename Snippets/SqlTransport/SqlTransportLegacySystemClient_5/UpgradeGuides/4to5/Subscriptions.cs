using System;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class UpgradeSubscriptions
{
    void Cache(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-subscription-caching

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var subscriptions = transport.SubscriptionSettings();

        subscriptions.CacheSubscriptionInformationFor(TimeSpan.FromMinutes(1));

        // OR

        subscriptions.DisableSubscriptionCache();

        #endregion
    }

    void ConfigureSubscriptionTable(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-subscription-table

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var subscriptions = transport.SubscriptionSettings();

        subscriptions.SubscriptionTableName(
            tableName: "Subscriptions", 
            schemaName: "OptionalSchema",
            catalogName: "OptionalCatalog");

        #endregion
    }
}