using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shared;

const string EndpointName = "Samples.OpenTelemetry.Publisher";

Console.Title = EndpointName;

#region jaeger-exporter-configuration
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(EndpointName))
    .AddSource("NServiceBus.Core")
    .AddJaegerExporter()
    .Build();
#endregion

var endpointConfiguration = new EndpointConfiguration(EndpointName);

endpointConfiguration.UsePersistence<LearningPersistence>();
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