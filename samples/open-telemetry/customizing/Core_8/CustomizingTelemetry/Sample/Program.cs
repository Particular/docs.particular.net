using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

Console.Title = "CustomTelemetry";

#region open-telemetry-config
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService("CustomTelemetry");

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource("NServiceBus.Core")
    .AddSource(CustomActivitySources.Name)
    .AddProcessor(new NetHostProcessor())
    .AddConsoleExporter()
    .Build();
#endregion

var endpointConfiguration = new EndpointConfiguration("CustomTelemetry");
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.Pipeline.Register(
    new AddOrderIdToTrace(),
    "Adds order id to the ambient trace for incoming ShipOrder messages"
);

endpointConfiguration.Pipeline.Register(
    new TraceBillingActivities(),
    "Traces incoming billing messages"
);

endpointConfiguration.Pipeline.Register(
    new TraceOutgoingOrderIdsBehavior(),
    "Captures outgoing order ids as trace tags"
);

var endpoint = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Endpoint started. Press any key to send a message. Press ESC to stop.");

var done = false;
while(!done)
{
    Console.WriteLine("Press ESC to stop.\nO. to create an order");
    switch(Console.ReadKey(true).Key)
    {
        case ConsoleKey.Escape:
            done = true;
            break;
        case ConsoleKey.O:
            await endpoint.SendLocal(new CreateOrder { OrderId = Guid.NewGuid() });
            break;
        default:
            // Do nothing
            break;
    }
    
}

await endpoint.Stop();
Console.WriteLine("Endpoint stopped");

