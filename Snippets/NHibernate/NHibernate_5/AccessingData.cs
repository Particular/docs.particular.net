using NHibernate;
using NServiceBus;

class AccessingData
{
    public class OrderMessage :
        IMessage
    {
    }

    public class Order
    {
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
}