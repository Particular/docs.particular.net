using NServiceBus;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;

Console.Title = "MyEndpoint";

#region open-telemetry-config

var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(serviceName: "MyEndpoint", serviceInstanceId: Environment.MachineName);

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource("NServiceBus.*")
    .AddSource(CustomActivitySources.Name)
    .AddConsoleExporter()
    .Build();

#endregion

#region enable-opentelemetry

var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
endpointConfiguration.EnableOpenTelemetry();

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

endpointConfiguration.Pipeline.Register(new TraceOutgoingMessageSizeBehavior(), "Captures body size of outgoing messages as OpenTelemetry tags");

var endpoint = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Endpoint started.");

var done = false;
while (!done)
{
    Console.WriteLine("Press ESC to stop.\nO. to create an order");
    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.Escape:
            done = true;
            break;
        case ConsoleKey.O:
            await endpoint.SendLocal(new CreateOrder { OrderId = Guid.NewGuid() });
            break;
    }
}

await endpoint.Stop();

Console.WriteLine("Endpoint stopped");