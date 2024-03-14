using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Mvc.Server";
        var endpointConfiguration = new EndpointConfiguration("Samples.Mvc.Server");
        endpointConfiguration.EnableCallbacks(makesRequests: false);
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}