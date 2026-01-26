using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var endpointConfiguration = new EndpointConfiguration("Samples.Aurora.Lambda.ClientUI");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var transport = endpointConfiguration.UseTransport<SqsTransport>();
var routing = transport.Routing();
routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.Aurora.Lambda.Sales");

Console.WriteLine("Starting...");

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press [Enter] to place an order.");
Console.WriteLine("Press any other key to exit.");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var orderId = Guid.NewGuid().ToString("N");
    await messageSession.Send(new PlaceOrder() { OrderId = orderId });
    Console.WriteLine($"Order {orderId} was placed.");
}

await host.StopAsync();
