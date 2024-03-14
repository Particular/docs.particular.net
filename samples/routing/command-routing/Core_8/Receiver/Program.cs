using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.CommandRouting.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.CommandRouting.Receiver");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}