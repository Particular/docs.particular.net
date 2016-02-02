using System;
using System.Threading.Tasks;
using NHibernate;
using NServiceBus;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);

        #region StoreUserData

        ISession nhibernateSession = context.SynchronizedStorageSession.Session();
        nhibernateSession.Save(new Order
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