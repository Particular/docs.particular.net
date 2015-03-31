using System;
using NServiceBus;
using Raven.Client;

#region PlaceOrderHandler
public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    readonly IDocumentSession session;

    public PlaceOrderHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public void Handle(PlaceOrder message)
    {
        session.Store(new Order { OrderNumber = message.OrderNumber, OrderValue = message.OrderValue });

        Console.Out.WriteLine("Order {0} stored",message.OrderNumber);
    }
}
#endregion
