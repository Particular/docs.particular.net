#region PlaceDelayedOrderHandler
using System;
using System.Threading.Tasks;
using NServiceBus;

public class PlaceDelayedOrderHandler : IHandleMessages<PlaceDelayedOrder>
{
    public Task Handle(PlaceDelayedOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine(@"[Defer Message Delivery] Order for Product:{0} placed with id: {1}", message.Product, message.Id);
        return Task.FromResult(0);
    }
}

#endregion
