using System;
using NServiceBus;
using NHibernate;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    IBus bus;
    ISession session;

    public OrderSubmittedHandler(IBus bus, ISession session)
    {
        this.bus = bus;
        this.session = session;
    }

    public void Handle(OrderSubmitted message)
    {
        Console.WriteLine($"Order {message.OrderId} worth {message.Value} submitted");

        session.Save(new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        });

        bus.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });
    }
}