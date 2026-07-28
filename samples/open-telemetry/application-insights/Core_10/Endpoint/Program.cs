using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using NServiceBus.Transport.AzureServiceBus;

var endpointName = "Samples.OpenTelemetry.AppInsights";

Console.Title = endpointName;

var attributes = new Dictionary<string, object>
{
    ["service.name"] = endpointName,
    ["service.instance.id"] = Guid.NewGuid().ToString(),
};

var appInsightsConnectionString = "<YOUR APP INSIGHTS CONNECTION STRING HERE>";
var asbConnectionString = "<YOUR AZURE SERVICE BUS CONNECTION STRING HERE>";

var resourceBuilder = ResourceBuilder.CreateDefault().AddAttributes(attributes);

#region enable-tracing

// OTel path: captures both NServiceBus spans AND Azure SDK spans, exports to AI via OTel exporter
var traceProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource("NServiceBus.Core*")
    .AddSource("Azure.*")  // captures Azure.Messaging.ServiceBus SDK spans
    .AddAzureMonitorTraceExporter(o => o.ConnectionString = appInsightsConnectionString)
    .AddConsoleExporter()
    .Build();

#endregion

// Legacy AI path: DependencyTrackingTelemetryModule also captures Azure SDK calls
// and exports them to AI independently - producing duplicates of the Azure SDK spans above
var aiConfig = TelemetryConfiguration.CreateDefault();
aiConfig.ConnectionString = appInsightsConnectionString;
var dependencyModule = new DependencyTrackingTelemetryModule();
dependencyModule.Initialize(aiConfig);

#region enable-meters

var meterProvider = Sdk.CreateMeterProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddMeter("NServiceBus.Core*")
    .AddAzureMonitorMetricExporter(o => o.ConnectionString = appInsightsConnectionString)
    .AddConsoleExporter()
    .Build();

#endregion

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new AzureServiceBusTransport(asbConnectionString, TopicTopology.Default));

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
using var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

var simulator = new LoadSimulator(messageSession, TimeSpan.Zero, TimeSpan.FromSeconds(10));
simulator.Start();

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
    traceProvider?.Dispose();
    meterProvider?.Dispose();
    dependencyModule?.Dispose();
    aiConfig?.Dispose();
}