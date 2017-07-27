using System;
using System.Diagnostics;
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

        var envInstrumentationKey = "ApplicationInsightKey";
        var instrumentationKey = Environment.GetEnvironmentVariable(envInstrumentationKey);

        if (string.IsNullOrEmpty(instrumentationKey))
        {
            throw new Exception($"Environment variable '{envInstrumentationKey}' required.");
        }

        Console.WriteLine("Using application insight application key: {0}", instrumentationKey);

        TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;

        TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = Debugger.IsAttached;

        var instance = $"{DateTime.UtcNow.Ticks}@{Dns.GetHostName()}";
        var probeCollector = new ApplicationInsightProbeCollector(Console.Title, instance);

        // TODO: See https://github.com/Particular/NServiceBus.Metrics/issues/41

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.RegisterObservers(probeCollector.Register);

        var tokenSource = new CancellationTokenSource();
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var simulator = LoadSimulator(endpointInstance, tokenSource.Token);

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
            probeCollector.Flush();
        }
    }

    // Simulates busy (almost no delay) / quite time (10 seconds delay) in a sine wave.
    static async Task LoadSimulator(IEndpointInstance endpointInstance, CancellationToken token)
    {
        try
        {
            for (var angle = 0;; angle++)
            {
                await endpointInstance.SendLocal(new SomeCommand())
                    .ConfigureAwait(false);

                var angleInRadians = Math.PI * angle / 180.0;
                // 0 - 10 seconds
                var delay = (int) (5000 * Math.Sin(angleInRadians));
                delay += 5000;
                await Task.Delay(delay, token)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}
