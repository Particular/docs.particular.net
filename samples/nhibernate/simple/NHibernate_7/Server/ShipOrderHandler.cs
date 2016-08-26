using System;
using System.Threading.Tasks;
using NServiceBus;

public class ShipOrderHandler :
    IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession.Session();
        var orderShipped = new Server.OrderShipped
        {
            Id = message.OrderId,
            ShippingDate = DateTime.UtcNow,
        };

        session.Save(orderShipped);

        return Task.FromResult(true);
    }
}
