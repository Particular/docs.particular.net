using NServiceBus;
using System;
using System.Reflection;

#region WorkerHandler

public class ProcessOrderCommandHandler : IHandleMessages<PlaceOrder>
{
    IBus bus;

    public ProcessOrderCommandHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(PlaceOrder placeOrder)
    {
        // Process Order...
        Console.WriteLine("Processing received order....");

        OrderPlaced message = new OrderPlaced
        {
            OrderId = placeOrder.OrderId,
            WorkerName = Assembly.GetEntryAssembly().GetName().Name
        };
        bus.Reply(message);
        Console.WriteLine("Sent Order placed event for orderId [{0}].", placeOrder.OrderId);
    }
}

#endregion
