#region PlaceOrderHandler
using System;
using System.Threading.Tasks;
using NServiceBus;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine(@"Order for Product:{0} placed with id: {1}", message.Product, message.Id);

        Console.WriteLine(@"Publishing: OrderPlaced for Order Id: {0}", message.Id);

        OrderPlaced orderPlaced = new OrderPlaced
                                  {
                                      OrderId = message.Id
                                  };
        await context.Publish(orderPlaced);
    }
}

#endregion
