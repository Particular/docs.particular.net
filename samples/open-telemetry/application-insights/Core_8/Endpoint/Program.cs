using Azure.Monitor.OpenTelemetry.Exporter;
using NServiceBus;
using System;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Collections.Generic;
using System.Threading;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using OpenTelemetry.Metrics;

class Program
{
    const string EndpointName = "Samples.OpenTelemetry.AppInsights";

    static async Task Main(string[] args)
    {
        Console.Title = EndpointName;

        var attributes = new Dictionary<string, object>
        {
            ["service.name"] = EndpointName,
            ["service.instance.id"] = Guid.NewGuid().ToString(),
        };

        var appInsightsConnectionString = "<YOUR KEY HERE>";

        var resourceBuilder = ResourceBuilder.CreateDefault().AddAttributes(attributes);

        #region enable-tracing

        var traceProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource("NServiceBus.Core")
            .AddAzureMonitorTraceExporter(o => o.ConnectionString = appInsightsConnectionString)
            .AddConsoleExporter()
            .Build();

        #endregion

        #region enable-meters

        var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        telemetryConfiguration.ConnectionString = appInsightsConnectionString;
        var telemetryClient = new TelemetryClient(telemetryConfiguration);
        telemetryClient.Context.GlobalProperties["Endpoint"] = EndpointName;

        var meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter("NServiceBus.Core")
            .AddNServiceBusTelemetryClientExporter(telemetryClient)
            .AddConsoleExporter()
            .Build();
        #endregion

        #region enable-open-telemetry
        var endpointConfiguration = new EndpointConfiguration(EndpointName);
        endpointConfiguration.EnableOpenTelemetry();
        #endregion

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();
        var cancellation = new CancellationTokenSource();
        var endpointInstance = await Endpoint.Start(endpointConfiguration, cancellation.Token);

        var simulator = new LoadSimulator(endpointInstance, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        simulator.Start(cancellation.Token);

        try
        {
            Console.WriteLine("Endpoint started. Press any key to send a message. Press ESC to stop");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                await endpointInstance.SendLocal(new SomeMessage(), cancellation.Token);
            }
        }
        finally
        {
            await simulator.Stop(cancellation.Token);
            await endpointInstance.Stop(cancellation.Token);
            traceProvider?.Dispose();
            meterProvider?.Dispose();
        }
    }
}
