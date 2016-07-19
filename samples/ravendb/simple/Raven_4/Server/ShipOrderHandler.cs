using System;
using System.Threading.Tasks;
using NServiceBus;

#region handler

public class ShipOrderHandler :
    IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession.RavenSession();
        var orderShipped = new OrderShipped
        {
            Id = message.OrderId,
            ShippingDate = DateTime.UtcNow,
        };
        return session.StoreAsync(orderShipped);
    }
}

#endregion