namespace Snippets6.Persistence.NHibernate
{
    using System.Threading.Tasks;
    using global::NHibernate;
    using NServiceBus;
    using NServiceBus.Persistence;

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

        public class Directly
        {
            public void Config()
            {
                #region NHibernateAccessingDataDirectlyConfig

                BusConfiguration busConfiguration = new BusConfiguration();
                busConfiguration.UsePersistence<NHibernatePersistence>()
                    .RegisterManagedSessionInTheContainer();

                #endregion
            }

            #region NHibernateAccessingDataDirectly

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

            #region CustomSessionCreation

            public void Configure()
            {
                BusConfiguration busConfiguration = new BusConfiguration();
                busConfiguration.UsePersistence<NHibernatePersistence>()
                    .UseCustomSessionCreationMethod(CreateSession);
            }

            ISession CreateSession(ISessionFactory sessionFactory, string connectionString)
            {
                return sessionFactory.OpenSession();
            }
            #endregion

        }
    }
}