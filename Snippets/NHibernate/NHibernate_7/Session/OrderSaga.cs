using System.Threading.Tasks;
using NServiceBus;

namespace NHibernate_7.Session
{
    #region NHibernateAccessingDataViaContextSaga

    public class OrderSaga :
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