using System;
using NServiceBus;
using NServiceBus.RavenDB.Persistence;
using Raven.Client;

#region handler    

public class ShipOrderHandler : IHandleMessages<ShipOrder>
{
    ISessionProvider sessionProvider;

    public ShipOrderHandler(ISessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public void Handle(ShipOrder message)
    {
        IDocumentSession session = sessionProvider.Session;
        session.Store(new OrderShipped
        {
            Id = message.OrderId,
            ShippingDate = DateTime.UtcNow,
        });
    }
}

#endregion