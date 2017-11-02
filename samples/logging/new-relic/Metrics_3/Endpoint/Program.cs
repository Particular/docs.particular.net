using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Metrics.Tracing.Endpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.Metrics.Tracing.Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #region newrelic-load-simulator

        var simulator = new LoadSimulator(endpointInstance, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        simulator.Start();

        #endregion

        try
        {
            Console.WriteLine("Endpoint started. Press 'enter' to send a message");
            Console.WriteLine("Press ESC key to quit");

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }

                await endpointInstance.SendLocal(new SomeCommand())
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            await simulator.Stop()
                .ConfigureAwait(false);
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}