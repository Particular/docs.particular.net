using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();
    IBus bus;

    public OrderSubmittedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(OrderSubmitted message)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");
        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        bus.Reply(orderAccepted);
    }
}
