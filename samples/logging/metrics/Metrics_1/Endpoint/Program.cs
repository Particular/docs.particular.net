using System;
using System.Diagnostics;
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

        #region EnableMetricTracing

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.RegisterObservers(
            register: context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(
                        observer: length =>
                        {
                            Trace.WriteLine($"Duration: '{duration.Name}'. Value: '{length}'");
                        });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: () =>
                        {
                            Trace.WriteLine($"Signal: '{signal.Name}'");
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