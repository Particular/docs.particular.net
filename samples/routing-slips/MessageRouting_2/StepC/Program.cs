using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageRouting.RoutingSlips;

class Program
{
    static void Main()
    {
        RunBus().GetAwaiter().GetResult();
    }

    static async Task RunBus()
    {
        Console.Title = "Samples.RoutingSlips.StepC";
        var endpointConfiguration = new EndpointConfiguration("Samples.RoutingSlips.StepC");

        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableFeature<RoutingSlips>();

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}
