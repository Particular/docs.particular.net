#region PlaceDelayedOrderHandler
using System;
using NServiceBus;

public class PlaceDelayedOrderHandler : IHandleMessages<PlaceDelayedOrder>
{
    public void Handle(PlaceDelayedOrder message)
    {
        Console.WriteLine(@"[Defer Message Delivery] Order for Product:{0} placed with id: {1}", message.Product, message.Id);
    }
}

#endregion
