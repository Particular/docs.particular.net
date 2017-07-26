using System;
using System.Net;
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

        TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;

        if (System.Diagnostics.Debugger.IsAttached)
        {
            TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;
        }

        var instance = $"{DateTime.UtcNow.Ticks}@{Dns.GetHostName()}";
        var x = new ApplicationInsightProbeCollector(Console.Title, instance);

        var metricsOptions = endpointConfiguration.EnableMetrics();
        metricsOptions.RegisterObservers(x.Register);

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
