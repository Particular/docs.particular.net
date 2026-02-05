using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shared;

class Program
{
    const string EndpointName = "Samples.OpenTelemetry.Publisher";

    public static async Task Main()
    {
        Console.Title = EndpointName;

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

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press '1' to publish the OrderReceived event");
        Console.WriteLine("Press any other key to exit");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderReceivedId = Guid.NewGuid();
            if (key.Key == ConsoleKey.D1)
            {
                var orderReceived = new OrderReceived
                {
                    OrderId = orderReceivedId
                };
                await endpointInstance.Publish(orderReceived);
                Console.WriteLine($"Published OrderReceived Event with Id {orderReceivedId}.");
            }
            else
            {
                break;
            }
        }

        await endpointInstance.Stop();
        tracerProvider.ForceFlush();
    }
}
