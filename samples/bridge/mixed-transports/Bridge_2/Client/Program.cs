using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Bridge.MixedTransports.Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.MixedTransports.Client");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        #region ClientBridgeConfig

        var bridge = transport.Routing().ConnectToBridge("Samples.Bridge.MixedTransports.BridgeLeft");
        bridge.RouteToEndpoint(typeof(MyMessage), "Samples.Bridge.MixedTransports.Server");
        bridge.RegisterPublisher(typeof(MyEvent), "Samples.Bridge.MixedTransports.Server");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press <enter> to send a message");
        while (true)
        {
            Console.ReadLine();
            var id = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
            var message = new MyMessage
            {
                Id = id
            };
            await endpointInstance.Send(message)
                .ConfigureAwait(false);
        }
    }
}