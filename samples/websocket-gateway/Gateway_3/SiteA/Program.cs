using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "SiteA";

        #region WebSocketGateway-EndpointConfig-SiteA

        var config = new EndpointConfiguration("Custom Gateway - SiteA");
        config.UseTransport<LearningTransport>();
        // NOTE: The LearningPersistence does not support the gateway
        config.UsePersistence<InMemoryPersistence>();

        var gatewaySettings = config.Gateway();
        gatewaySettings.ChannelFactories(
            s => new WebSocketChannelSender(),
            s => new WebSocketChannelReceiver()
        );

        #endregion

        gatewaySettings.AddReceiveChannel(
            address: "ws://localhost:33334/SiteA",
            type: "WebSocket",
            isDefault: true);

        gatewaySettings.AddSite(
            siteKey: "SiteB",
            address: "ws://localhost:33335/SiteB",
            type: "WebSocket");

        var endpoint = await Endpoint.Start(config).ConfigureAwait(false);

        await endpoint.SendToSites(new[] { "SiteB" }, new SomeMessage { Contents = "Hello, World!" }).ConfigureAwait(false);

        Console.WriteLine("Started SiteA. Sent message to SiteB");

        Console.ReadLine();

        await endpoint.Stop().ConfigureAwait(false);
    }
}