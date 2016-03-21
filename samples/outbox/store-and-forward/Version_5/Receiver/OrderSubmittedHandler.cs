using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();
    IBus bus;

    public OrderSubmittedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(OrderSubmitted message)
    {
        log.InfoFormat("Order {0} worth {1} submitted", message.OrderId, message.Value);
        bus.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });
    }
}
