using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.CommandRouting.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.CommandRouting.Sender");

        #region configure-command-route
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();

        routing.RouteToEndpoint(
            messageType: typeof(PlaceOrder),
            destination: "Samples.CommandRouting.Receiver"
        );
        #endregion


        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press S to send an order");
        Console.WriteLine("Press C to cancel an order");
        Console.WriteLine("Press ESC to exit");

        var keyPressed = Console.ReadKey(true).Key;

        while (keyPressed != ConsoleKey.Escape)
        {
            switch (keyPressed)
            {
                case ConsoleKey.S:
                    await PlaceOrder(endpointInstance, Guid.NewGuid().ToString(), 25m);
                    break;
                case ConsoleKey.C:
                    await CancelOrder(endpointInstance, Guid.NewGuid().ToString());
                    break;
            }
            keyPressed = Console.ReadKey(true).Key;
        }

        await endpointInstance.Stop();
    }

    static async Task PlaceOrder(IEndpointInstance endpointInstance, string orderId, decimal value)
    {
        #region send-command-with-configured-route
        var command = new PlaceOrder
        {
            OrderId = orderId,
            Value = value
        };

        await endpointInstance.Send(command);
        #endregion
    }

    static async Task CancelOrder(IEndpointInstance endpointInstance, string orderId)
    {
        #region send-command-without-configured-route
        var command = new CancelOrder
        {
            OrderId = orderId
        };

        await endpointInstance.Send("Samples.CommandRouting.Receiver", command);
        #endregion
    }
}