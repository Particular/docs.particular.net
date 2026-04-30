using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

const string EndpointName = "Samples.OpenTelemetry.Subscriber";

Console.Title = EndpointName;
var builder = Host.CreateApplicationBuilder(args);

var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(EndpointName))
    .AddSource("NServiceBus.Core*")
    .AddOtlpExporter() // The exporter defaults to gRPC on over port 4317 - https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md#otlpexporteroptions
    .Build();

var endpointConfiguration = new EndpointConfiguration(EndpointName);
endpointConfiguration.EnableOpenTelemetry();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.RunAsync();
tracerProvider.ForceFlush();