using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Left Sender";

Console.WriteLine("Starting...");

var builder = Host.CreateDefaultBuilder(args);

builder.UseNServiceBus(x =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.LeftSender");
    endpointConfiguration.UsePersistence<LearningPersistence>();

    endpointConfiguration.Conventions().DefiningCommandsAs(t => t.Name == "PlaceOrder");
    endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
    endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    var routing = endpointConfiguration.UseTransport(new LearningTransport());
    routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.Bridge.RightReceiver");

    endpointConfiguration.SendFailedMessagesTo("error");
    endpointConfiguration.EnableInstallers();

    return endpointConfiguration;
});

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press '1' to send the PlaceOrder command");
Console.WriteLine("Press '2' to publish the OrderReceived event");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();
    if (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2)
    {
        break;
    }

    var orderId = Guid.NewGuid();

    if (key.Key == ConsoleKey.D1)
    {
        var placeOrder = new PlaceOrder
        {
            OrderId = orderId
        };
        await messageSession.Send(placeOrder);
        Console.WriteLine($"Send PlaceOrder Command with Id {orderId}");
        continue;
    }

    // Key D2 pressed
    var orderReceived = new OrderReceived
    {
        OrderId = orderId
    };

    await messageSession.Publish(orderReceived);
    Console.WriteLine($"Published OrderReceived Event with Id {orderId}.");
}

await host.StopAsync();