using System;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client;

#region handler    

public class ShipOrderHandler : IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        
        IAsyncDocumentSession session = context.GetRavenSession(); ;
        return session.StoreAsync(new OrderShipped
        {
            Id = message.OrderId,
            ShippingDate = DateTime.UtcNow,
        });
    }
}

#endregion