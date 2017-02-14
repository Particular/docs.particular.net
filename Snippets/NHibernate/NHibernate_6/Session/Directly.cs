namespace NHibernate_6.Session
{
    using NHibernate;
    using NServiceBus;
    using NServiceBus.Persistence;

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

        public class OrderHandler :
            IHandleMessages<OrderMessage>
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
            persistence.UseCustomSessionCreationMethod(
                callback: (sessionFactory, connectionString) =>
                {
                    return sessionFactory.OpenSession();
                });

            #endregion
        }
    }
}