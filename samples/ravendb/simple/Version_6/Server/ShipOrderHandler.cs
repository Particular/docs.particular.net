using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.RavenDB.Persistence;
using Raven.Client;

#region handler    

public class ShipOrderHandler : IHandleMessages<ShipOrder>
{
    IAsyncSessionProvider sessionProvider;

    public ShipOrderHandler(IAsyncSessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        IAsyncDocumentSession session = sessionProvider.AsyncSession;
        return session.StoreAsync(new OrderShipped
        {
            Id = message.OrderId,
            ShippingDate = DateTime.UtcNow,
        });
    }
}

#endregion