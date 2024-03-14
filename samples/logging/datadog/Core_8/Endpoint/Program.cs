using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    const string EndpointName = "Samples.Metrics.Tracing.Endpoint";

    static async Task Main()
    {
        Console.Title = EndpointName;
        var endpointConfiguration = new EndpointConfiguration(EndpointName);

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        DataDogMetrics.Setup(endpointConfiguration);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        var simulator = new LoadSimulator(endpointInstance, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        await simulator.Start();

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

                await endpointInstance.SendLocal(new SomeCommand());
            }
        }
        finally
        {
            await simulator.Stop();
            await endpointInstance.Stop();
        }
    }
}
