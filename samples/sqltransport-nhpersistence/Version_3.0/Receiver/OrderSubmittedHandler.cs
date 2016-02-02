using System;
using System.Threading.Tasks;
using NServiceBus;
using NHibernate;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    ISession session;

    public OrderSubmittedHandler(ISession session)
    {
        this.session = session;
    }


    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);

        #region StoreUserData

        session.Save(new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        });

        #endregion

        #region Reply

        await context.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });

        #endregion
    }
}