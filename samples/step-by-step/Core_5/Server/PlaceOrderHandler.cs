using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region PlaceOrderHandler

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
    IBus bus;

    public PlaceOrderHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(PlaceOrder message)
    {
        log.Info($"Order for Product:{message.Product} placed with id: {message.Id}");
        log.Info($"Publishing: OrderPlaced for Order Id: {message.Id}");

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.Id
        };
        bus.Publish(orderPlaced);
    }
}

#endregion
