using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.DelayedDelivery.Client";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.DelayedDelivery.Client");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await SendOrder(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    static async Task SendOrder(IEndpointInstance endpointInstance)
    {

        Console.WriteLine("Press '1' to send PlaceOrder - defer message handling");
        Console.WriteLine("Press '2' to send PlaceDelayedOrder - defer message delivery");
        Console.WriteLine("Press enter key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            Guid id = Guid.NewGuid();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    #region SendOrder
                    PlaceOrder placeOrder = new PlaceOrder
                    {
                        Product = "New shoes",
                        Id = id
                    };
                    await endpointInstance.Send("Samples.DelayedDelivery.Server", placeOrder);
                    Console.WriteLine("[Defer Message Handling] Sent a new PlaceOrder message with id: {0}", id.ToString("N"));
                    #endregion
                    continue;
                case ConsoleKey.D2:
                    #region DeferOrder
                    PlaceDelayedOrder placeDelayedOrder = new PlaceDelayedOrder
                    {
                        Product = "New shoes",
                        Id = id
                    };
                    SendOptions options = new SendOptions();

                    options.SetDestination("Samples.DelayedDelivery.Server");
                    options.DelayDeliveryWith(TimeSpan.FromSeconds(5));
                    await endpointInstance.Send(placeDelayedOrder, options);
                    Console.WriteLine("[Defer Message Delivery] Deferred a new PlaceDelayedOrder message with id: {0}", id.ToString("N"));
                    #endregion
                    continue;
                case ConsoleKey.Enter:
                    return;
                default:
                    return;
            }
        }

    }
}
