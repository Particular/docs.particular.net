using System.Threading.Tasks;
using NServiceBus;

namespace NHibernate_8.Session
{
    #region NHibernateAccessingDataViaContextHandler

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