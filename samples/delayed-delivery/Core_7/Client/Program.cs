using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Client";
        var endpointConfiguration = new EndpointConfiguration("Samples.DelayedDelivery.Client");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await SendOrder(endpointInstance);
        await endpointInstance.Stop();
    }

    static async Task SendOrder(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press '1' to send PlaceOrder - defer message handling");
        Console.WriteLine("Press '2' to send PlaceDelayedOrder - defer message delivery");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            var id = Guid.NewGuid();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    #region SendOrder
                    var placeOrder = new PlaceOrder
                    {
                        Product = "New shoes",
                        Id = id
                    };
                    await endpointInstance.Send("Samples.DelayedDelivery.Server", placeOrder);
                    Console.WriteLine($"[Defer Message Handling] Sent a PlaceOrder message with id: {id.ToString("N")}");
                    #endregion
                    continue;
                case ConsoleKey.D2:
                    #region DeferOrder
                    var placeDelayedOrder = new PlaceDelayedOrder
                    {
                        Product = "New shoes",
                        Id = id
                    };
                    var options = new SendOptions();

                    options.SetDestination("Samples.DelayedDelivery.Server");
                    options.DelayDeliveryWith(TimeSpan.FromSeconds(5));
                    await endpointInstance.Send(placeDelayedOrder, options);
                    Console.WriteLine($"[Defer Message Delivery] Deferred a PlaceDelayedOrder message with id: {id.ToString("N")}");
                    #endregion
                    continue;
                default:
                    return;
            }
        }

    }
}
