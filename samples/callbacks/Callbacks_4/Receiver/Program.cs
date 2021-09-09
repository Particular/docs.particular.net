using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Callbacks.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.Callbacks.Receiver");
        endpointConfiguration.UsePersistence<LearningPersistence>();     
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.EnableCallbacks(makesRequests: false);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}