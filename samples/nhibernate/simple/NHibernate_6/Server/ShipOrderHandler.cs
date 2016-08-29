using System;
using NHibernate;
using NServiceBus;

#region Handler
public class ShipOrderHandler :
    IHandleMessages<ShipOrder>
{
    private readonly ISession session;

    public void Handle(ShipOrder message)
    {
        var orderShipped = new Server.OrderShipped
        {
            Id = message.OrderId,
            ShippingDate = DateTime.UtcNow,
        };

        session.Save(orderShipped);
    }

    public ShipOrderHandler(ISession session)
    {
        this.session = session;
    }
}
#endregion