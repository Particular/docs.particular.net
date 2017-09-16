using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Gateway.RemoteSite";
        var endpointConfiguration = new EndpointConfiguration("Samples.Gateway.RemoteSite");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableFeature<Gateway>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}