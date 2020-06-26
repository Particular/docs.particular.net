using System.Threading.Tasks;
using NServiceBus;

namespace NHibernate_8.Session
{
    using NHibernate;

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


    public class EndpointWithSessionRegistered
    {
        public void Configure(EndpointConfiguration config)
        {
            #region AccessingDataConfigureISessionDI

            config.RegisterComponents(c =>
            {
                c.ConfigureComponent(b =>
                {
                    var session = b.Build<INHibernateStorageSession>();
                    var repository = new MyRepository(session.Session);
                    return repository;
                }, DependencyLifecycle.InstancePerUnitOfWork);
            });

            #endregion
        }

        public class MyRepository
        {
            public MyRepository(ISession session)
            {
                throw new System.NotImplementedException();
            }
        }
    }

}