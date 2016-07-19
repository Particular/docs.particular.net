using NServiceBus;
using NHibernate;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();
    IBus bus;
    ISession session;

    public OrderSubmittedHandler(IBus bus, ISession session)
    {
        this.bus = bus;
        this.session = session;
    }

    public void Handle(OrderSubmitted message)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        session.Save(order);

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        bus.Reply(orderAccepted);
    }
}