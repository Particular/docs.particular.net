using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
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

        const string EnvInstrumentationKey = "ApplicationInsightKey";
        var instrumentationKey = Environment.GetEnvironmentVariable(EnvInstrumentationKey);

        if (string.IsNullOrEmpty(instrumentationKey))
        {
            Console.WriteLine($"Please set environment variable '{EnvInstrumentationKey}' to application insight instrumentation key.");
            return;
        }

        Console.WriteLine("Using application insight application key: {0}", instrumentationKey);

        TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;

        if (System.Diagnostics.Debugger.IsAttached)
        {
            TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;
        }

        var instance = $"{DateTime.UtcNow.Ticks}@{Dns.GetHostName()}";
        var x = new ApplicationInsightProbeCollector(Console.Title, instance);

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.RegisterObservers(x.Register);

        var tokenSource = new CancellationTokenSource();
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var simulator = LoadSimilator(endpointInstance, tokenSource.Token);

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
            tokenSource.Cancel();
            await simulator
                .ConfigureAwait(false);
            await endpointInstance.Stop()
                .ConfigureAwait(false);
            x.Flush();
        }
    }

    /// <summary>
    /// Simulates busy (almost no delay) / quite time (10 seconds delay) in a sine wave.
    /// </summary>
    /// <param name="endpointInstance"></param>
    /// <returns></returns>
    static async Task LoadSimilator(IEndpointInstance endpointInstance, CancellationToken token)
    {
        try
        {
            for (int angle = 0;; angle++)
            {
                await endpointInstance.SendLocal(new SomeCommand())
                    .ConfigureAwait(false);

                var angleInRadians = Math.PI * angle / 180.0;
                // 0 - 10 seconds
                int delay = (int) (5000 * Math.Sin(angleInRadians));
                delay += 5000;
                Console.WriteLine("Delay {0}ms for angle {1}", delay, angle);
                await Task.Delay(delay, token)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}
