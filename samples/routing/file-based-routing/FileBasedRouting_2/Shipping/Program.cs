using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.FileBasedRouting;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.FileBasedRouting.Shipping";
        var endpointConfiguration = new EndpointConfiguration("Samples.FileBasedRouting.Shipping");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("Samples.FileBasedRouting.Error");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        routing.UseFileBasedRouting(@"..\..\..\..\endpoints.xml");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}