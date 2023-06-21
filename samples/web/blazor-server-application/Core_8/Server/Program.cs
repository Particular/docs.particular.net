using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    public static async Task Main()
    {
        var endpointConfiguration = new EndpointConfiguration(Console.Title = "Samples.Blazor.Server");
        endpointConfiguration.EnableCallbacks(makesRequests: false);
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
