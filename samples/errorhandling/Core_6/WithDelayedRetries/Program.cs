using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ErrorHandling.WithDelayedRetries";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);

        var endpointConfiguration = new EndpointConfiguration("Samples.ErrorHandling.WithDelayedRetries");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
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
            await endpointInstance.SendLocal(myMessage)
                .ConfigureAwait(false);
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}