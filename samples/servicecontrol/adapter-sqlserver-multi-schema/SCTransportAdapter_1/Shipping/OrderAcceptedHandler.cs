using NServiceBus;
using NServiceBus.Logging;

class OrderAcceptedHandler :
    IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();
    ChaosGenerator chaos;
    IBus bus;

    public OrderAcceptedHandler(ChaosGenerator chaos, IBus bus)
    {
        this.chaos = chaos;
        this.bus = bus;
    }

    public void Handle(OrderAccepted message)
    {
        log.Info($"Shipping order {message.OrderId} for {message.Value}");
        chaos.Invoke();
        var orderShipped = new OrderShipped
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        bus.Publish(orderShipped);
    }
}