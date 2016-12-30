using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        Start().GetAwaiter().GetResult();
    }

    static async Task Start()
    {
        Console.Title = "Samples.SagaMigration.Client";

        var config = new EndpointConfiguration("Samples.SagaMigration.Client");
        config.UsePersistence<InMemoryPersistence>();
        config.SendFailedMessagesTo("error");

        var endpoint = await Endpoint.Start(config).ConfigureAwait(false);

        while (true)
        {
            Console.WriteLine("Type 'start <SagaId>' or 'ping <SagaId>'.");
            var line = Console.ReadLine();
            if (line == null)
            {
                continue;
            }
            var parts = line.Split(' ');
            if (parts.Length != 2)
            {
                continue;
            }
            var sagaId = parts[1];
            if (string.Equals(parts[0], "start", StringComparison.OrdinalIgnoreCase))
            {
                var startingMessage = new StartingMessage
                {
                    SomeId = sagaId
                };
                await endpoint.Send("Samples.SagaMigration.Server", startingMessage)
                    .ConfigureAwait(false);
                Console.WriteLine($"Sent a starting message to {sagaId}");
            }
            else if (string.Equals(parts[0], "ping", StringComparison.OrdinalIgnoreCase))
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