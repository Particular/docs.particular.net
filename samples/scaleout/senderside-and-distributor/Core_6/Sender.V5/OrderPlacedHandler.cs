using NServiceBus;
using NServiceBus.Logging;

public class OrderPlacedHandler :
    IHandleMessages<PlaceOrderResponse>
{
    IBus bus;
    static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

    public OrderPlacedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(PlaceOrderResponse placeOrderResponse)
    {
        log.Info($"Received OrderPlaced. OrderId: {placeOrderResponse.OrderId}. Worker: {placeOrderResponse.WorkerName}");
        var confirmOrder = new ConfirmOrder
        {
            OrderId = placeOrderResponse.OrderId
        };
        bus.Reply(confirmOrder);
    }
}
