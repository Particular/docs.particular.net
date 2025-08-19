using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;

public class ShipOrderHandler(ILogger<ShipOrderHandler> logger) :
    IHandleMessages<ShipOrder>
{
    public async Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        #region DynamoDBStorageSession

        var orderShippingInformation = new OrderShippingInformation
        {
            Id = Guid.NewGuid(),
            OrderId = message.OrderId,
            ShippedAt = DateTimeOffset.UtcNow,
        };

        var session = context.SynchronizedStorageSession.DynamoPersistenceSession();

        session.Add(new TransactWriteItem
        {
            Put = new Put
            {
                TableName = "Samples.DynamoDB.Transactions",
                Item = orderShippingInformation.ToMap()
            }
        });

        #endregion

        logger.LogInformation("Order Shipped. OrderId {OrderId}", message.OrderId);

        await context.Publish(new OrderShipped
        {
            OrderId = orderShippingInformation.OrderId,
            ShippingDate = orderShippingInformation.ShippedAt
        });
    }

}