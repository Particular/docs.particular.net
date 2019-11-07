using NServiceBus;
using NServiceBus.Transport.SQLServer;

class ConfigureSubscriptionTable
{
    public ConfigureSubscriptionTable(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-configure-subscription-table

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var subscriptions = transport.SubscriptionSettings();

        subscriptions.SubscriptionTableName(
            tableName: "Subscriptions", 
            schemaName: "OptionalSchema",
            catalogName: "OptionalCatalog");

        #endregion
    }
}