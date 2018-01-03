using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "SiteB";

        #region WebSocketGateway-EndpointConfig-SiteB

        var config = new EndpointConfiguration("Custom Gateway - SiteB");
        var transport = config.UseTransport<LearningTransport>();
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(SomeMessage), "Custom Gateway - SiteB");
        // NOTE: The LearningPersistence does not support the gateway
        config.UsePersistence<InMemoryPersistence>();

        var gatewaySettings = config.Gateway();
        gatewaySettings.ChannelFactories(
            s => new WebSocketChannelSender(),
            s => new WebSocketChannelReceiver()
        );

        #endregion

        var endpoint = await Endpoint.Start(config).ConfigureAwait(false);

        Console.WriteLine("Started SiteB");

        Console.ReadLine();

        await endpoint.Stop().ConfigureAwait(false);
    }
}