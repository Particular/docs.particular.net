using NServiceBus;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        #region enable-opentelemetry-metrics
        var meterProviderBuilder = Sdk.CreateMeterProviderBuilder()
            .AddMeter("NServiceBus.Core");
        #endregion

        #region enable-prometheus-exporter
        meterProviderBuilder.AddPrometheusHttpListener(options => options.UriPrefixes = new[] { "http://127.0.0.1:9464" });
        #endregion

        var meterProvider = meterProviderBuilder.Build();

        #region enable-opentelemetry
        var config = new EndpointConfiguration("Samples.OpenTelemetry.Metrics");
        config.EnableOpenTelemetry();
        #endregion
        config.UseSerialization<SystemJsonSerializer>();
        config.UseTransport<LearningTransport>();

        var cancellation = new CancellationTokenSource();
        var endpointInstance = await Endpoint.Start(config, cancellation.Token);

        #region prometheus-load-simulator

        var simulator = new LoadSimulator(endpointInstance, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        simulator.Start(cancellation.Token);

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

                await endpointInstance.SendLocal(new SomeCommand(), cancellation.Token);
            }
        }
        finally
        {
            await simulator.Stop(cancellation.Token);
            await endpointInstance.Stop(cancellation.Token);
            meterProvider?.Dispose();
        }
    }
}
