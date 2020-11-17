using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.AzureTable;

public class ShipOrderHandler :
    IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        var orderShippingInformation = new OrderShippingInformation
        {
            Id = Guid.NewGuid(),
            OrderId = message.OrderId,
            ShippedAt = DateTimeOffset.UtcNow,
        };

        Store(orderShippingInformation, context);

        Log.Info($"Order Shipped. OrderId {message.OrderId}");

        return context.Reply(new OrderShipped { OrderId = orderShippingInformation.OrderId, ShippingDate = orderShippingInformation.ShippedAt });
    }

    private static void Store(OrderShippingInformation orderShippingInformation, IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession.AzureTablePersistenceSession();

        orderShippingInformation.PartitionKey = session.PartitionKey;

        session.Batch.Add(TableOperation.Insert(orderShippingInformation));
    }

    static ILog Log = LogManager.GetLogger<ShipOrderHandler>();
}