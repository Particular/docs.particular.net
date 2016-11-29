using NServiceBus;
using NServiceBus.Logging;

public class OrderPlacedHandler :
    IHandleMessages<OrderPlaced>
{
    IBus bus;
    static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

    public OrderPlacedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(OrderPlaced orderPlaced)
    {
        log.Info($"Received OrderPlaced. OrderId: {orderPlaced.OrderId}. Worker: {orderPlaced.WorkerName}");
        var confirmOrder = new ConfirmOrder
        {
            OrderId = orderPlaced.OrderId
        };
        bus.Reply(confirmOrder);
    }
}
