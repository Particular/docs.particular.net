using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

class Program
{
    const string EndpointName = "Samples.OpenTelemetry.Subscriber";

    public static async Task Main()
    {
        Console.Title = EndpointName;

        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(EndpointName))
            .AddSource("NServiceBus.Core*")
            .AddOtlpExporter() // The exporter defaults to gRPC on over port 4317 - https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md#otlpexporteroptions
            .Build();

        var endpointConfiguration = new EndpointConfiguration(EndpointName);

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        _ = Console.ReadKey();

        await endpointInstance.Stop();
        tracerProvider.ForceFlush();
    }
}
