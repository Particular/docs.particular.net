namespace NHibernate_6
{
    using NHibernate;
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class AccessingData
    {
        class OrderMessage : IMessage
        {
        }

        class Order
        {
        }

        class ViaContext
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

        class Directly
        {
            void Config(BusConfiguration busConfiguration)
            {
                #region NHibernateAccessingDataDirectlyConfig

                var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
                persistence.RegisterManagedSessionInTheContainer();

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


            void Configure(BusConfiguration busConfiguration)
            {
                #region CustomSessionCreation

                var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
                persistence.UseCustomSessionCreationMethod((sessionFactory, connectionString) =>
                    sessionFactory.OpenSession());

                #endregion
            }
        }
    }
}