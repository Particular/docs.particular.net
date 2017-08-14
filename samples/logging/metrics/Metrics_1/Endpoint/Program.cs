using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Metrics.Tracing.Endpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.Metrics.Tracing.Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region EnableMetricTracing

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.RegisterObservers(context =>
        {
            foreach (var duration in context.Durations)
            {
                duration.Register(durationLength =>
                {
                    Trace.WriteLine($"Duration '{duration.Name}' value observed: '{durationLength}'");
                });
            }
            foreach (var signal in context.Signals)
            {
                signal.Register(() =>
                {
                    Trace.WriteLine($"'{signal.Name}' occurred.");
                });
            }
        });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        try
        {
            Console.WriteLine("Endpoint started. Press 'enter' to send a message");
            Console.WriteLine("Press any other key to quit");

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }

                await endpointInstance.SendLocal(new SomeCommand())
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}