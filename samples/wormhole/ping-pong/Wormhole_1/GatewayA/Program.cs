using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Transports.Http;
using NServiceBus.Wormhole.Gateway;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Wormhole.PingPong.GatewayA";

        var gatewayConfig = new WormholeGatewayConfiguration<MsmqTransport, HttpTransport>("Gateway.SiteA", "SiteA");

        #region ConfigureGatewayA

        gatewayConfig.ConfigureRemoteSite("SiteB", "Gateway.SiteB");

        #endregion

        //Hack necessary to make 6.3.x MSMQ work
        gatewayConfig.CustomizeLocalTransport(
            customization: (configuration, transportExtensions) =>
            {
                var settings = transportExtensions.GetSettings();
                settings.Set("errorQueue", "poison");
            });

        var gateway = await gatewayConfig.Start()
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await gateway.Stop()
            .ConfigureAwait(false);
    }
}