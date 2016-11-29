using NServiceBus;
using NServiceBus.Logging;

public class OrderPlacedHandler :
    IHandleMessages<OrderPlaced>
{
    static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

    public IBus Bus { get; set; }

    public void Handle(OrderPlaced orderPlaced)
    {
        log.Info($"Received OrderPlaced. OrderId: {orderPlaced.OrderId}. Worker: {orderPlaced.WorkerName}");
        Bus.Reply(new ConfirmOrder
        {
            OrderId = orderPlaced.OrderId
        });
    }
}
