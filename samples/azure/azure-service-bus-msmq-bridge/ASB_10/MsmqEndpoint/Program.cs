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
        endpointConfiguration.AddDeserializer<NewtonsoftSerializer>();
        var routing = endpointConfiguration.UseTransport<MsmqTransport>().Routing();

        var bridge = routing.ConnectToBridge("Bridge");
        bridge.RegisterPublisher(typeof(OtherEvent), "Samples.Azure.ServiceBus.AsbEndpoint");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}