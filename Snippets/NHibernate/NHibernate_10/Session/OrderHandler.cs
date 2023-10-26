using System.Threading.Tasks;
using NServiceBus;

namespace NHibernate_9.Session
{
    using Microsoft.Extensions.DependencyInjection;
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
            synchronizedStorageSession.Session.Save(new Order());
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
                c.AddScoped<MyRepository, MyRepository>(svc =>
                {
                    var session = svc.GetService<INHibernateStorageSession>();
                    var repository = new MyRepository(session.Session);
                    return repository;
                });
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