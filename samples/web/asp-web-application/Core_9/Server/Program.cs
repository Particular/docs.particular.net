using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    public static async Task Main()
    {
        var endpointConfiguration = new EndpointConfiguration(Console.Title = "Samples.AsyncPages.Server");
        endpointConfiguration.EnableCallbacks(makesRequests: false);
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
