namespace Snippets6.Persistence.NHibernate
{
    using System.Threading.Tasks;
    using global::NHibernate;
    using NServiceBus;

    public class AccessingData
    {
        public class OrderMessage : IMessage
        {
        }

        public class Order
        {
        }

        public class ViaContext
        {
            #region NHibernateAccessingDataViaContext
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
    }
}