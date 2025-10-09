using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "LeftSender";

var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.LeftSender");
endpointConfiguration.UsePersistence<LearningPersistence>();

endpointConfiguration.Conventions().DefiningCommandsAs(t => t.Name == "PlaceOrder");
endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var routing = endpointConfiguration.UseTransport(new LearningTransport());
routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.Bridge.RightReceiver");

endpointConfiguration.SendFailedMessagesTo("error");

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

Console.WriteLine("Press '1' to send the PlaceOrder command");
Console.WriteLine("Press '2' to publish the OrderReceived event");
Console.WriteLine("Press 'esc' key to exit");

var messageSession = host.Services.GetRequiredService<IMessageSession>();

var keepGoing = true;

while (keepGoing)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    var orderId = Guid.NewGuid();
    switch (key.Key)
    {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
            var placeOrder = new PlaceOrder
            {
                OrderId = orderId
            };
            await messageSession.Send(placeOrder);
            Console.WriteLine($"Send PlaceOrder Command with Id {orderId}");
            break;
        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
            var orderReceived = new OrderReceived
            {
                OrderId = orderId
            };
            await messageSession.Publish(orderReceived);
            Console.WriteLine($"Published OrderReceived Event with Id {orderId}.");
            break;
        case ConsoleKey.Escape:
            keepGoing = false;
            break;
    }
}

await host.StopAsync();