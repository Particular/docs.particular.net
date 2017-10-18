using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Azure.ServiceBus.MsmqEndpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.MsmqEndpoint");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<XmlSerializer>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        #region connect-msmq-side-of-bridge

        var routing = transport.Routing();
        var bridge = routing.ConnectToBridge("Bridge-MSMQ");

        #endregion

        #region route-command-via-bridge

        bridge.RouteToEndpoint(typeof(MyCommand), "Samples.Azure.ServiceBus.AsbEndpoint");

        #endregion
        
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press 'c' to send a command");
        Console.WriteLine("Press 'e' to publish an event");
        Console.WriteLine("Press any other key to exit");
        var @continue = true;

        while (@continue)
        {
            var key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.C:
                    await endpointInstance.Send(new MyCommand { Property = "command from MSMQ endpoint"}).ConfigureAwait(false);
                    Console.WriteLine("\nCommand sent");
                    break;

                case ConsoleKey.E:
                    await endpointInstance.Publish<MyEvent>(@event => { @event.Property = "event from MSMQ endpoint"; }).ConfigureAwait(false);
                    Console.WriteLine("\nEvent sent");
                    break;

                default:
                    @continue = false;
                    break;
            }
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}