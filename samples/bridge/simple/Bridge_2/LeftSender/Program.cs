using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Bridge.LeftSender";
        var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.LeftSender");
        endpointConfiguration.UsePersistence<LearningPersistence>();

        endpointConfiguration.Conventions().DefiningCommandsAs(t => t.Name == "PlaceOrder");
        endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
        endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

        var routing = endpointConfiguration.UseTransport(new LearningTransport());
        routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.Bridge.RightReceiver");

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Start(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press '1' to send the PlaceOrder command");
        Console.WriteLine("Press '2' to publish the OrderReceived event");
        Console.WriteLine("Press 'esc' other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderId = Guid.NewGuid();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    var placeOrder = new PlaceOrder
                    {
                        OrderId = orderId
                    };
                    await endpointInstance.Send(placeOrder)
                        .ConfigureAwait(false);
                    Console.WriteLine($"Send PlaceOrder Command with Id {orderId}");
                    break;
                case ConsoleKey.D2:
                    var orderReceived = new OrderReceived
                    {
                        OrderId = orderId
                    };
                    await endpointInstance.Publish(orderReceived)
                        .ConfigureAwait(false);
                    Console.WriteLine($"Published OrderReceived Event with Id {orderId}.");
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }
    }
}