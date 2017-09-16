using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Wormhole;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Wormhole.PingPong.Server";
        var endpointConfiguration = new EndpointConfiguration(
            "Samples.Wormhole.PingPong.Server");

        endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));
        recoverability.DisableLegacyRetriesSatellite();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        #region ConfigureServer

        endpointConfiguration.UseWormholeGateway("Gateway.SiteB");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}