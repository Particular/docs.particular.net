using System;
using System.Threading.Tasks;
using NServiceBus;

#region Handler
public class ShipOrderHandler :
    IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession.Session();
        var orderShipped = new OrderShipped
        {
            Id = message.OrderId,
            ShippingDate = DateTime.UtcNow,
        };

        session.Save(orderShipped);

        return Task.CompletedTask;
    }
}
#endregion