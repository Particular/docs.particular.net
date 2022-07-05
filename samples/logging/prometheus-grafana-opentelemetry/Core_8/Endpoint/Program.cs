using System;
using System.Threading.Tasks;
using NServiceBus;
using OpenTelemetry;
using OpenTelemetry.Metrics;

public class Program
{
    public static async Task Main()
    {
        #region enable-prometheus-exporter
        var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter("NServiceBus.Diagnostics")
            .AddPrometheusExporter(opt =>
            {
                opt.StartHttpListener = true;
                opt.HttpListenerPrefixes = new[]
                {
                    "http://localhost:9185",
                    "http://192.168.0.114:9184"
                };
                opt.ScrapeEndpointPath = "/metrics";
            })
            .Build();

        #endregion

        var config = new EndpointConfiguration("Samples.OpenTelemetry.Metrics");
        config.UseTransport<LearningTransport>();
        config.UsePersistence<LearningPersistence>();
        config.MakeInstanceUniquelyAddressable("main");

        #region enable-opentelemetry-metrics

        config.EnableOpenTelemetryMetrics();

        #endregion

        var endpointInstance = await Endpoint.Start(config);

        #region prometheus-load-simulator

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

                await endpointInstance.SendLocal(new SomeCommand());
            }
        }
        finally
        {
            await simulator.Stop();
            await endpointInstance.Stop();
            meterProvider?.Dispose();
        }
    }
}