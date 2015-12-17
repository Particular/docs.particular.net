using System;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client;

#region PlaceOrderHandler
public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    IDocumentSession session;

    public PlaceOrderHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        Order order = new Order
        {
            OrderNumber = message.OrderNumber, 
            OrderValue = message.OrderValue
        };
        session.Store(order);

        Console.WriteLine("Order {0} stored",message.OrderNumber);
        return Task.FromResult(0);
    }
}
#endregion
