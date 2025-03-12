using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class ShipOrderHandler(ILogger<ShipOrderHandler> logger) :
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

        logger.LogInformation($"Order Shipped. OrderId {message.OrderId}");

        var options = new PublishOptions();
        options.SetHeader("Sample.CosmosDB.Transaction.OrderId", message.OrderId.ToString());

        return context.Publish(new OrderShipped { OrderId = orderShippingInformation.OrderId, ShippingDate = orderShippingInformation.ShippedAt }, options);
    }

    private static void Store(OrderShippingInformation orderShippingInformation, IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession.CosmosPersistenceSession();
        var requestOptions = new TransactionalBatchItemRequestOptions
        {
            EnableContentResponseOnWrite = false,
        };

        session.Batch.CreateItem(orderShippingInformation, requestOptions);
    }


}