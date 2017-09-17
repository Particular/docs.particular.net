using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SagaMigration.Client";

        var endpointConfiguration = new EndpointConfiguration("Samples.SagaMigration.Client");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        while (true)
        {
            Console.WriteLine("Type 'start <SagaId>', 'complete <SagaId>' or hit enter to exit.");
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                break;
            }
            var parts = line.ToLowerInvariant().Split(' ');
            if (parts.Length != 2)
            {
                continue;
            }
            var sagaId = parts[1];
            if (parts[0] == "start")
            {
                var startingMessage = new StartingMessage
                {
                    SomeId = sagaId
                };
                await endpoint.Send("Samples.SagaMigration.Server", startingMessage)
                    .ConfigureAwait(false);
                Console.WriteLine($"Sent a starting message to {sagaId}");
            }
            else if (parts[0] == "complete")
            {
                var correlatedMessage = new CorrelatedMessage
                {
                    SomeId = sagaId
                };
                await endpoint.Send("Samples.SagaMigration.Server", correlatedMessage)
                    .ConfigureAwait(false);
                Console.WriteLine($"Sent a correlated message to {sagaId}");
            }
        }
    }
}