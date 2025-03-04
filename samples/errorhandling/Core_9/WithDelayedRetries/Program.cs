using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

static class Program
{
    static async Task Main()
    {
        Console.Title = "WithDelayedRetries";
        var endpointConfiguration = new EndpointConfiguration("Samples.ErrorHandling.WithDelayedRetries");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press enter to send a message that will throw an exception.");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            var myMessage = new MyMessage
            {
                Id = Guid.NewGuid()
            };
            await endpointInstance.SendLocal(myMessage);
        }
        await endpointInstance.Stop();
    }
}
