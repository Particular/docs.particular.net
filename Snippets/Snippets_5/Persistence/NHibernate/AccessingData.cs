using System;
using NHibernate;
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
            public NHibernateStorageContext DataContext { get; set; }

            public void Handle(OrderMessage message)
            {
                DataContext.Session.Save(new Order());
            }
        }
        #endregion
    }

    public class Directly
    {
        public void Config()
        {
            #region NHibernateAccessingDataDirectlyConfig

            BusConfiguration busConfig = new BusConfiguration();
            busConfig.UsePersistence<NHibernatePersistence>().RegisterManagedSessionInTheContainer();

            #endregion
        }

        #region NHibernateAccessingDataDirectly

        public class OrderHandler : IHandleMessages<OrderMessage>
        {
            public NHibernateStorageContext DataContext { get; set; }

            public void Handle(OrderMessage message)
            {
                DataContext.Session.Save(new Order());
            }
        }

        #endregion

        #region CustomSessionCreation

        public void Configure()
        {
            BusConfiguration busConfig = new BusConfiguration();
            busConfig.UsePersistence<NHibernatePersistence>().UseCustomSessionCreationMethod(CreateSession);
        }

        ISession CreateSession(ISessionFactory sessionFactory, string connectionString)
        {
            return sessionFactory.OpenSession();
        }
        #endregion

    }
}