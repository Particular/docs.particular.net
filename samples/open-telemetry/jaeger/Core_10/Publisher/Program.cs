using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shared;

const string EndpointName = "Samples.OpenTelemetry.Publisher";

Console.Title = EndpointName;
var builder = Host.CreateApplicationBuilder(args);

#region jaeger-exporter-configuration
var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(EndpointName))
    .AddSource("NServiceBus.Core*")
    .AddOtlpExporter() // The exporter defaults to gRPC on over port 4317 - https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md#otlpexporteroptions
    .Build();
#endregion

var endpointConfiguration = new EndpointConfiguration(EndpointName);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press '1' to publish the OrderReceived event");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.D1)
    {
        var orderReceived = new OrderReceived
        {
            OrderId = Guid.NewGuid()
        };

        await messageSession.Publish(orderReceived);
        Console.WriteLine($"Published OrderReceived Event with Id {orderReceived.OrderId}.");
    }
    else
    {
        break;
    }
}

await host.StopAsync();
tracerProvider.ForceFlush();