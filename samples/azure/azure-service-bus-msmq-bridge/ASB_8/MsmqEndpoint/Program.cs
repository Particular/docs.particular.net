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

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}