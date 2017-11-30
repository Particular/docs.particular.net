using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.FaultTolerance.Client";
        var endpointConfiguration = new EndpointConfiguration("Samples.FaultTolerance.Client");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            var id = Guid.NewGuid();
            var myMessage = new MyMessage
            {
                Id = id
            };
            await endpointInstance.Send("Samples.FaultTolerance.Server", myMessage)
                .ConfigureAwait(false);

            Console.WriteLine($"Sent a message with id: {id:N}");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
