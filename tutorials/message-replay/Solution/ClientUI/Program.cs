using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "ClientUI";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var routing = endpointConfiguration.UseTransport<LearningTransport>().Routing();
routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

while (true)
{
    Console.WriteLine("Press 'P' to place an order, or any other key to quit.");
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.P)
    {
        break;
    }

    var command = new PlaceOrder
    {
        OrderId = Guid.NewGuid().ToString()
    };

    // Send the command
    await messageSession.Send(command);
    Console.WriteLine($"Sending PlaceOrder command, OrderId = {command.OrderId}");
}

await host.StopAsync();