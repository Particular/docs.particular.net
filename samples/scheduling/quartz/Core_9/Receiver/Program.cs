using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.QuartzScheduler.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.QuartzScheduler.Receiver");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for messages from the Sender");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}