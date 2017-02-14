using NServiceBus;
using NServiceBus.Persistence.NHibernate;

namespace NHibernate_6.Session
{

    #region NHibernateAccessingDataViaContextSaga

    public class OrderSaga :
        IHandleMessages<OrderMessage>
    {
        NHibernateStorageContext dataContext;

        public OrderHandler(NHibernateStorageContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Handle(OrderMessage message)
        {
            var nhibernateSession = dataContext.Session;
            nhibernateSession.Save(new Order());
        }
    }

    #endregion
}