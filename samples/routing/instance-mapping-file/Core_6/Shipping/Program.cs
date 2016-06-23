using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomRouting.Shipping";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomRouting.Shipping");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.DistributeMessagesUsingFileBasedEndpointInstanceMapping(@"..\..\..\instance-mapping.xml");
        transport.RegisterPublisherForType("Samples.CustomRouting.Sales", typeof(OrderAccepted));
        transport.SimulateMultipleMachines("BackEnd");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}