using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MsgPack;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Serialization.MsgPack";
        #region config
        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.MsgPack");
        endpointConfiguration.UseSerialization<MsgPackSerializer>();
        #endregion
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        #region message
        var message = new CreateOrder
        {
            OrderId = 9,
            Date = DateTime.Now,
            CustomerId = 12,
            OrderItems = new List<OrderItem>
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
                },
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