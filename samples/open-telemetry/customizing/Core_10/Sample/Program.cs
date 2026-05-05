using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

endpointConfiguration.Pipeline.Register(new TraceOutgoingMessageSizeBehavior(), "Captures body size of outgoing messages as OpenTelemetry tags");
endpointConfiguration.Pipeline.Register(new TraceCustomExceptionInHandlerBehavior(), "Captures custom exception and sets reason code as a tag");

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

Console.WriteLine("Endpoint started.");

var done = false;
while (!done)
{
    Console.WriteLine("Press ESC to stop.\nO. to create an order. \nF. to create an order that fails processing.");
    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.Escape:
            done = true;
            break;
        case ConsoleKey.O:
            await messageSession.SendLocal(new CreateOrder { OrderId = Guid.NewGuid() });
            break;
        case ConsoleKey.F:
            await messageSession.SendLocal(new CreateOrder { OrderId = Guid.NewGuid(), SimulateFailure = true });
            break;
    }
}

await host.StopAsync();

Console.WriteLine("Endpoint stopped");