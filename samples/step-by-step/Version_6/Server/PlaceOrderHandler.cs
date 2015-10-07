#region PlaceOrderHandler
using System;
using System.Threading.Tasks;
using NServiceBus;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    IBus bus;

    public PlaceOrderHandler(IBus bus)
    {
        this.bus = bus;
    }

    public async Task Handle(PlaceOrder message)
    {
        Console.WriteLine(@"Order for Product:{0} placed with id: {1}", message.Product, message.Id);

        Console.WriteLine(@"Publishing: OrderPlaced for Order Id: {0}", message.Id);

        OrderPlaced orderPlaced = new OrderPlaced
                                  {
                                      OrderId = message.Id
                                  };
        await bus.PublishAsync(orderPlaced);
    }
}

#endregion
