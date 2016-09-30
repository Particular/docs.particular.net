using System.Threading.Tasks;
using NServiceBus;

class AccessingData
{
    public class OrderMessage :
        IMessage
    {
    }

    public class Order
    {
    }

    #region NHibernateAccessingDataViaContext

    public class OrderHandler :
        IHandleMessages<OrderMessage>
    {
        public Task Handle(OrderMessage message, IMessageHandlerContext context)
        {
            var nhibernateSession = context.SynchronizedStorageSession.Session();
            nhibernateSession.Save(new Order());
            return Task.CompletedTask;
        }
    }

    #endregion
}