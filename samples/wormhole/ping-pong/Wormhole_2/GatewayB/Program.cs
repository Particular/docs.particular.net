using NServiceBus;
using NServiceBus.Transports.Http;
using NServiceBus.Wormhole.Gateway;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Wormhole.PingPong.GatewayB";

        var gatewayConfig = new WormholeGatewayConfiguration<MsmqTransport, HttpTransport>("Gateway.SiteB.2", "SiteB");

        #region ConfigureGatewayB

        gatewayConfig.ConfigureRemoteSite("SiteA", "Gateway.SiteA.2");
        gatewayConfig.ForwardToEndpoint("Shared", "Samples.Wormhole.PingPong.Server");

        #endregion

        var gateway = await gatewayConfig.Start()
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await gateway.Stop()
            .ConfigureAwait(false);
    }
}