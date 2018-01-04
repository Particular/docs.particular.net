using NServiceBus;
using NServiceBus.Transports.Http;
using NServiceBus.Wormhole.Gateway;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Wormhole.PingPong.GatewayA";

        var gatewayConfig = new WormholeGatewayConfiguration<MsmqTransport, HttpTransport>("Gateway.SiteA", "SiteA");

        #region ConfigureGatewayA

        gatewayConfig.ConfigureRemoteSite("SiteB", "Gateway.SiteB");

        #endregion

        var gateway = await gatewayConfig.Start()
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await gateway.Stop()
            .ConfigureAwait(false);
    }
}