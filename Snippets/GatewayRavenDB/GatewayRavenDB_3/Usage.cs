using System;
using NServiceBus;
using Raven.Client.Documents;

class Usage
{
    public void DefaultUsage(EndpointConfiguration endpointConfiguration)
    {
        #region DefaultUsage

        var gatewayConfiguration = new RavenGatewayDeduplicationConfiguration((builder, _) =>
        {
            var documentStore = new DocumentStore
            {
                Urls = new[] { "database-server-url" },
                Database = "default-database-name"
            };

            documentStore.Initialize();

            return documentStore;
        })
        {
            // When running in a cluster, enable cluster wide transaction support
            EnableClusterWideTransactions = true,
        };

        var gatewaySettings = endpointConfiguration.Gateway(gatewayConfiguration);
        #endregion
    }

    public void CustomExpiration(RavenGatewayDeduplicationConfiguration gatewayConfiguration)
    {
        #region CustomExpiration
        gatewayConfiguration.DeduplicationDataTimeToLive = TimeSpan.FromDays(15);
        gatewayConfiguration.FrequencyToRunDeduplicationDataCleanup = 86400;
        #endregion
    }
}