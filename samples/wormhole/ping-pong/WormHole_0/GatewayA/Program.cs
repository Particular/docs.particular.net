using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transports.Http;
using NServiceBus.WormHole;
using NServiceBus.WormHole.Gateway;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.WormHole.PingPong.GatewayA";

        var gatewayConfig = new WormHoleGatewayConfiguration<MsmqTransport, HttpTransport>("Gateway.SiteA", "SiteA");
        gatewayConfig.ConfigureRemoteSite("SiteB", "Gatewat.SiteB");

        var gateway = await gatewayConfig.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await gateway.Stop().ConfigureAwait(false);
    }
}