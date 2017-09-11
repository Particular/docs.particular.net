using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

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

        config.EnableFeature<Gateway>();
        var gatewaySettings = config.Gateway();
        gatewaySettings.ChannelFactories(
            s => new WebSocketChannelSender(),
            s => new WebSocketChannelReceiver()
        );

        #endregion

        var endpoint = await Endpoint.Start(config).ConfigureAwait(false);

        await endpoint.SendToSites(new[] {"SiteB"}, new SomeMessage {Contents = "Hello, World!"}).ConfigureAwait(false);

        Console.WriteLine("Started SiteA. Sent message to SiteB");

        Console.ReadLine();

        await endpoint.Stop().ConfigureAwait(false);
    }
}