using NHibernate;
using NServiceBus;

namespace NHibernate_5.Session
{

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
}