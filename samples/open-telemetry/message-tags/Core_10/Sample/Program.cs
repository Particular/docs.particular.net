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
    .AddConsoleExporter()
    .Build();

#endregion

var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

#region register-behaviors

endpointConfiguration.Pipeline.Register(
    new CaptureOrderTagsBehavior(),
    "Captures PlaceOrder body properties as OpenTelemetry tags");
endpointConfiguration.Pipeline.Register(
    new CaptureHeaderTagsBehavior(),
    "Captures the order priority header as an OpenTelemetry tag");

#endregion

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

Console.WriteLine("Endpoint started.");

var done = false;
while (!done)
{
    Console.WriteLine("Press ESC to stop.\nPress O to place an order.");
    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.Escape:
            done = true;
            break;
        case ConsoleKey.O:
            var options = new SendOptions();
            options.RouteToThisEndpoint();
            #region set-custom-header
            options.SetHeader("sample.order.priority", "high");
            #endregion
            await messageSession.Send(
                new PlaceOrder
                {
                    OrderId = Guid.NewGuid(),
                    CustomerId = "CUST-" + Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()
                },
                options);
            break;
    }
}

await host.StopAsync();
Console.WriteLine("Endpoint stopped");
