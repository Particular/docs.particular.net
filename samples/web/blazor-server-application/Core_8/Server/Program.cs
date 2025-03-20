using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    public static async Task Main()
    {
        Console.Title = "BlazorServer";
        var endpointConfiguration = new EndpointConfiguration("Samples.Blazor.Server");
        endpointConfiguration.EnableCallbacks(makesRequests: false);
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
