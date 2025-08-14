using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Sender";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.CommandRouting.Sender");

#region configure-command-route
var routing = endpointConfiguration.UseTransport(new LearningTransport());

routing.RouteToEndpoint(
    messageType: typeof(PlaceOrder),
    destination: "Samples.CommandRouting.Receiver"
);
#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

await host.StartAsync();
var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press S to send an order");
Console.WriteLine("Press C to cancel an order");
Console.WriteLine("Press ESC to exit");

ConsoleKey keyPressed = Console.ReadKey(true).Key;

while (keyPressed != ConsoleKey.Escape)
{
    switch (keyPressed)
    {
        case ConsoleKey.S:
            await PlaceOrder(messageSession, Guid.NewGuid().ToString(), 25m);
            break;
        case ConsoleKey.C:
            await CancelOrder(messageSession, Guid.NewGuid().ToString());
            break;
    }

    keyPressed = Console.ReadKey(true).Key;
}

await host.StopAsync();

static async Task PlaceOrder(IMessageSession messageSession, string orderId, decimal value)
{
    #region send-command-with-configured-route
    var command = new PlaceOrder
    {
        OrderId = orderId,
        Value = value
    };

    await messageSession.Send(command);
    Console.WriteLine($"Order placed: {orderId}");
    #endregion
}

static async Task CancelOrder(IMessageSession messageSession, string orderId)
{
    #region send-command-without-configured-route
    var command = new CancelOrder
    {
        OrderId = orderId
    };

    await messageSession.Send("Samples.CommandRouting.Receiver", command);
    Console.WriteLine($"Order canceled: {orderId}");
    #endregion
}

