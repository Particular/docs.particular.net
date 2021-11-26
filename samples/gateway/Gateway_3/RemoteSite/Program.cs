using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Gateway;
using Raven.Client.Documents;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Gateway.RemoteSite";
        var endpointConfiguration = new EndpointConfiguration("Samples.Gateway.RemoteSite");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region RemoteSiteGatewayConfig
        var gatewayConfiguration = new RavenGatewayDeduplicationConfiguration((builder, _) =>
        {
            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8081", "http://localhost:8082", "http://localhost:8083"},
                Database = "gateway-remotesite"
            };

            documentStore.Initialize();

            return documentStore;
        })
        {
            //EnableClusterWideTransactions = true
        };
        var gatewayConfig = endpointConfiguration.Gateway(gatewayConfiguration);
        gatewayConfig.AddReceiveChannel("http://localhost:25899/RemoteSite/");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}