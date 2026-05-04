using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;

var endpointName = "Samples.OpenTelemetry.MetricsShim";

Console.Title = endpointName;

var attributes = new Dictionary<string, object>
{
    ["service.name"] = endpointName,
    ["service.instance.id"] = Guid.NewGuid().ToString(),
};

var appInsightsConnectionString = "<YOUR CONNECTION STRING HERE>";

var resourceBuilder = ResourceBuilder.CreateDefault().AddAttributes(attributes);

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
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.EnableFeature<EmitNServiceBusMetrics>();

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
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
    meterProvider?.Dispose();
}