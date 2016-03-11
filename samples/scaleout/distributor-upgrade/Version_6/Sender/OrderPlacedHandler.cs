using System;
using NServiceBus;

public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
{
    public void Handle(OrderPlaced orderPlaced)
    {
        Console.WriteLine("Received OrderPlaced. OrderId: {0}. Worker: {1}", orderPlaced.OrderId, orderPlaced.WorkerName);
    }
}
