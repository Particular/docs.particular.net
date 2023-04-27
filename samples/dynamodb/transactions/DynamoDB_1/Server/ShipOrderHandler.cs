using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using NServiceBus;
using NServiceBus.Logging;

public class ShipOrderHandler :
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

        Log.Info($"Order Shipped. OrderId {message.OrderId}");

        await context.Publish(new OrderShipped
        {
            OrderId = orderShippingInformation.OrderId,
            ShippingDate = orderShippingInformation.ShippedAt
        });
    }

    static ILog Log = LogManager.GetLogger<ShipOrderHandler>();
}