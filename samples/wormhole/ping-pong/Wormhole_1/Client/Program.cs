using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Wormhole;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Wormhole.PingPong.Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.Wormhole.PingPong.Client");

        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));
        recoverability.DisableLegacyRetriesSatellite();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        #region ConfigureClient

        var wormholeRoutingSettings = endpointConfiguration.UseWormholeGateway("Gateway.SiteA");
        wormholeRoutingSettings.RouteToSite<Ping>("SiteB");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press <enter> to send a message");
        while (true)
        {
            Console.ReadLine();
            var id = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
            var message = new Ping();
            var options = new SendOptions();
            options.SetMessageId(id);
            await endpointInstance.Send(message, options)
                .ConfigureAwait(false);
        }
    }
}