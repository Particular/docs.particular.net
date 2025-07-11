using System;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Persistence.AzureTable;

#region UseHeader
public class ShipOrderHandler (ILogger<ShipOrderHandler> logger):
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


        logger.LogInformation("Order Shipped. OrderId {OrderId}", message.OrderId);

        var options = new PublishOptions();
        options.SetHeader("Sample.AzureTable.Transaction.OrderId", message.OrderId.ToString());

        return context.Publish(new OrderShipped { OrderId = orderShippingInformation.OrderId, ShippingDate = orderShippingInformation.ShippedAt }, options);
    }

    private static void Store(OrderShippingInformation orderShippingInformation, IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession.AzureTablePersistenceSession();

        orderShippingInformation.PartitionKey = session.PartitionKey;

        session.Batch.Add(new TableTransactionAction(TableTransactionActionType.Add, orderShippingInformation));
    }
}
#endregion