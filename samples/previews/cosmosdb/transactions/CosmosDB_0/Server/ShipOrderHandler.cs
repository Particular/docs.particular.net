using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region UseHeader
public class ShipOrderHandler :
    IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        Log.Info($"Order Shipped. OrderId {message.OrderId}");

        var options = new PublishOptions();
        options.SetHeader("Sample.CosmosDB.Transaction.OrderId", message.OrderId.ToString());

        return context.Publish(new OrderShipped { OrderId = message.OrderId, ShippingDate = DateTime.Today }, options);
    }

    static ILog Log = LogManager.GetLogger<ShipOrderHandler>();
}
#endregion