using System;
using NServiceBus;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    IBus bus;

    public OrderSubmittedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(OrderSubmitted message)
    {
        Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);
        bus.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });
    }
}
