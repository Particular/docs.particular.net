namespace Snippets5.Persistence.NHibernate
{
    using global::NHibernate;
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

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
                NHibernateStorageContext dataContext;

                public OrderHandler(NHibernateStorageContext dataContext)
                {
                    this.dataContext = dataContext;
                }

                public void Handle(OrderMessage message)
                {
                    dataContext.Session.Save(new Order());
                }
            }
            #endregion
        }

        public class Directly
        {
            public void Config()
            {
                BusConfiguration busConfiguration = new BusConfiguration();
                #region NHibernateAccessingDataDirectlyConfig

                busConfiguration.UsePersistence<NHibernatePersistence>()
                    .RegisterManagedSessionInTheContainer();

                #endregion
            }

            #region NHibernateAccessingDataDirectly

            public class OrderHandler : IHandleMessages<OrderMessage>
            {
                ISession session;

                public OrderHandler(ISession session)
                {
                    this.session = session;
                }

                public void Handle(OrderMessage message)
                {
                    session.Save(new Order());
                }
            }

            #endregion


            public void Configure()
            {
                BusConfiguration busConfiguration = new BusConfiguration();

                #region CustomSessionCreation

                busConfiguration.UsePersistence<NHibernatePersistence>()
                    .UseCustomSessionCreationMethod((sessionFactory, connectionString) =>
                        sessionFactory.OpenSession());

                #endregion
            }


        }
    }
}