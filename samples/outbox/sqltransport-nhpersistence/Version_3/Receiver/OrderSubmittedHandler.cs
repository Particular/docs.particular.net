using System;
using System.Threading.Tasks;
using NServiceBus;
using NHibernate;
using NServiceBus.Logging;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();
    static readonly Random ChaosGenerator = new Random();
    
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.InfoFormat("Order {0} worth {1} submitted", message.OrderId, message.Value);

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

        if (ChaosGenerator.Next(2) == 0)
        {
            throw new Exception("Boom!");
        }
    }
}