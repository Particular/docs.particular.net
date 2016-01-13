using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.RavenDB.Persistence;

#region handler    

public class ShipOrderHandler : IHandleMessages<ShipOrder>
{
    public async Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        var session = context.GetRavenSession();
        await session.StoreAsync(new OrderShipped
        {
            Id = message.OrderId,
            ShippingDate = DateTime.UtcNow,
        });
    }
}

#endregion