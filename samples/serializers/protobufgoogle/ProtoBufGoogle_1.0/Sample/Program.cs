using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.ProtoBufGoogle;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Serialization.ProtoBufGoogle";
        #region config
        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.ProtoBufGoogle");
        endpointConfiguration.UseSerialization<ProtoBufGoogleSerializer>();
        #endregion
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        #region messagesend

        var message = new CreateOrder
        {
            OrderId = 9,
            CustomerId = 12,
            OrderItems =
            {
                new OrderItem
                {
                    ItemId = 6,
                    Quantity = 2
                },
                new OrderItem
                {
                    ItemId = 5,
                    Quantity = 4
                }
            }
        };
        await endpointInstance.SendLocal(message)
            .ConfigureAwait(false);
        #endregion
        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}