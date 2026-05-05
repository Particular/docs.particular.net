using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

public class Program
{
    const string EndpointName = "Samples.OpenTelemetry.Metrics";
    public static async Task Main()
    {
        Console.Title = EndpointName;

        var attributes = new Dictionary<string, object>
        {
            ["service.name"] = EndpointName,
            ["service.instance.id"] = Guid.NewGuid().ToString(),
        };

        var resourceBuilder = ResourceBuilder.CreateDefault().AddAttributes(attributes);

        #region enable-opentelemetry-metrics
        var meterProviderBuilder = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter("NServiceBus.Core*");
        #endregion

        #region enable-prometheus-http-listener
        meterProviderBuilder.AddPrometheusHttpListener(options => options.UriPrefixes = new[] { "http://*:9464/" });
        #endregion

        var meterProvider = meterProviderBuilder.Build();

        var endpointConfiguration = new EndpointConfiguration(EndpointName);
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();
        await host.StartAsync();

        #region prometheus-load-simulator

        var simulator = new LoadSimulator(messageSession, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        simulator.Start();

        #endregion

        try
        {
            Console.WriteLine("Endpoint started. Press any key to send a message. Press ESC to stop");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {

                await messageSession.SendLocal(new SomeMessage());
            }
        }
        finally
        {
            simulator.Stop();
            await host.StopAsync();
            meterProvider?.Dispose();
        }
    }
}