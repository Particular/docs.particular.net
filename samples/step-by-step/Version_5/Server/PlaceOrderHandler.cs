using NServiceBus;
using NServiceBus.Logging;

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
        log.InfoFormat(@"Order for Product:{0} placed with id: {1}", message.Product, message.Id);

        log.InfoFormat(@"Publishing: OrderPlaced for Order Id: {0}", message.Id);

        OrderPlaced orderPlaced = new OrderPlaced
                                  {
                                      OrderId = message.Id
                                  };
        bus.Publish(orderPlaced);
    }
}

#endregion
