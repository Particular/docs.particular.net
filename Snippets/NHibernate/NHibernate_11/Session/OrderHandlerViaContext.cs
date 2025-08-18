namespace NHibernate_9.Session
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class OrderHandlerViaContext :
        IHandleMessages<OrderMessage>
    {
        #region NHibernateAccessingDataViaContextHandler

        public Task Handle(OrderMessage message, IMessageHandlerContext context)
        {
            var nhibernateSession = context.SynchronizedStorageSession.Session();
            nhibernateSession.Save(new Order());
            return Task.CompletedTask;
        }

        #endregion
    }
}