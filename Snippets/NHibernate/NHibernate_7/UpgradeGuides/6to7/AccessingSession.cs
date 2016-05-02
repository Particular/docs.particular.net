using System.Threading.Tasks;
using global::NHibernate;
using NServiceBus;

class AccessingSession
{
    public class OrderMessage : IMessage
    {
    }

    public class Order
    {
    }

    #region NHibernateAccessingSessionUpgrade6To7

    public class OrderHandler : IHandleMessages<OrderMessage>
    {
        public Task Handle(OrderMessage message, IMessageHandlerContext context)
        {
            ISession nhibernateSession = context.SynchronizedStorageSession.Session();
            nhibernateSession.Save(new Order());
            return Task.FromResult(0);
        }
    }

    #endregion

}