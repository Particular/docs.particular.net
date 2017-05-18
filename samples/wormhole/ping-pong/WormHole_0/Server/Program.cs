using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.WormHole;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.WormHole.PingPong.Server";
        var endpointConfiguration = new EndpointConfiguration(
            "Samples.WormHole.PingPong.Server");

        endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        endpointConfiguration.Recoverability()
            .Immediate(immediate => immediate.NumberOfRetries(0))
            .Delayed(delayed => delayed.NumberOfRetries(0))
            .DisableLegacyRetriesSatellite();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        endpointConfiguration.UseWormHoleGateway("Gateway.SiteB");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();
        
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}