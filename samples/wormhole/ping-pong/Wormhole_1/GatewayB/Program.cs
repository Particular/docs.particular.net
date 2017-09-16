﻿using System;
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

        var gatewayConfig = new WormholeGatewayConfiguration<MsmqTransport, HttpTransport>("Gateway.SiteB", "SiteB");

        #region ConfigureGatewayB

        gatewayConfig.ConfigureRemoteSite("SiteA", "Gateway.SiteA");
        gatewayConfig.ForwardToEndpoint("Shared", "Samples.Wormhole.PingPong.Server");

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