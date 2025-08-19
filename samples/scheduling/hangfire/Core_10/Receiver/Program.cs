using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.HangfireScheduler.Receiver");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for messages from the Scheduler");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}