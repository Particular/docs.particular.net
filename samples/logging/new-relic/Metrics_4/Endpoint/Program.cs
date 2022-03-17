using System;
using System.Threading.Tasks;
using Endpoint;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        var endpointConfiguration = new EndpointConfiguration(Console.Title = "Samples.Metrics.Tracing.Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        NewRelicMetrics.Setup(endpointConfiguration);

        var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #region newrelic-load-simulator

        var simulator = new LoadSimulator(endpointInstance, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        simulator.Start();

        #endregion

        try
        {
            Console.WriteLine("Endpoint started.");
            Console.WriteLine("Press [ENTER] to send additional messages.");
            Console.WriteLine("Press [ESC] to quit.");

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Escape:
                        return;
                    case ConsoleKey.Enter:
                        await endpointInstance.SendLocal(new SomeCommand())
                            .ConfigureAwait(false);
                        break;
                }
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