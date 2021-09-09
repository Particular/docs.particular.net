using NServiceBus;
using System;
using System.Threading.Tasks;
using NServiceBus.Gateway;

class Program
{
    static async Task Main()
    {
        Console.Title = "SiteB";

        #region WebSocketGateway-EndpointConfig-SiteB

        var config = new EndpointConfiguration("Custom Gateway - SiteB");
        var transport = config.UseTransport(new LearningTransport());
        transport.RouteToEndpoint(typeof(SomeMessage), "Custom Gateway - SiteB");

        var gatewaySettings = config.Gateway(new NonDurableDeduplicationConfiguration());
        gatewaySettings.ChannelFactories(
            s => new WebSocketChannelSender(),
            s => new WebSocketChannelReceiver()
        );

        gatewaySettings.AddReceiveChannel(
            address: "ws://localhost:33335/SiteB",
            type: "WebSocket",
            isDefault: true);

        gatewaySettings.AddSite(
            siteKey: "SiteA",
            address: "ws://localhost:33334/SiteA",
            type: "WebSocket");

        #endregion

        var endpoint = await Endpoint.Start(config).ConfigureAwait(false);

        Console.WriteLine("Started SiteB");

        Console.ReadLine();

        await endpoint.Stop().ConfigureAwait(false);
    }
}