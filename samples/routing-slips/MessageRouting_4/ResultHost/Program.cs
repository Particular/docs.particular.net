using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageRouting.RoutingSlips;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.RoutingSlips.ResultHost";
        var endpointConfiguration = new EndpointConfiguration("Samples.RoutingSlips.ResultHost");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableFeature<RoutingSlips>();

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}