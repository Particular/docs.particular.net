using NServiceBus;
using NServiceBus.Gateway.RavenDB;
using Raven.Client.Documents;

class Usage
{

    public void StandardUsage(EndpointConfiguration endpointConfiguration, string connectionString)
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
        });

        var gatewaySettings = endpointConfiguration.Gateway(gatewayConfiguration);
        #endregion
    }
}

