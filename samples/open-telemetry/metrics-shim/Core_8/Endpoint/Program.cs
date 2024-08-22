using Azure.Monitor.OpenTelemetry.Exporter;
using NServiceBus;
using System;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Collections.Generic;
using System.Threading;
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
    .AddMeter("NServiceBus.Core")
    .AddAzureMonitorMetricExporter(o => o.ConnectionString = appInsightsConnectionString)
    .AddConsoleExporter()
    .Build();

#endregion

#region enable-open-telemetry
var endpointConfiguration = new EndpointConfiguration(endpointName);
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
    meterProvider?.Dispose();
}
