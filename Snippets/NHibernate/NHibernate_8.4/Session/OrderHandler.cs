using System.Threading.Tasks;
using NServiceBus;

namespace NHibernate_8.Session
{
    #region NHibernateAccessingDataViaDI

    public class OrderHandler :
        IHandleMessages<OrderMessage>
    {
        INHibernateStorageSession synchronizedStorageSession;

        public OrderHandler(INHibernateStorageSession synchronizedStorageSession)
        {
            this.synchronizedStorageSession = synchronizedStorageSession;
        }

        public Task Handle(OrderMessage message, IMessageHandlerContext context)
        {
            var nhibernateSession = context.SynchronizedStorageSession.Session();
            nhibernateSession.Save(new Order());
            return Task.CompletedTask;
        }
    }

    #endregion

    #region AccessingDataConfigureISessionDI

    public class EndpointWithSessionRegistered
    {
        public void Configure(EndpointConfiguration config)
        {
            config.RegisterComponents(c =>
            {
                c.ConfigureComponent(b => b.Build<INHibernateStorageSession>().Session,
                    DependencyLifecycle.InstancePerUnitOfWork);
            });
        }
    }

    #endregion
}