namespace Snippets5.Outbox.NHibernate.SessionAccess
{
    using NServiceBus;
    using NServiceBus.Persistence.NHibernate;

    #region OutboxNHibernateAccessSession
    class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        NHibernateStorageContext nHibernateStorageContext;

        public OrderPlacedHandler(NHibernateStorageContext nHibernateStorageContext)
        {
            this.nHibernateStorageContext = nHibernateStorageContext;
        }

        public void Handle(OrderPlaced message)
        {
            OrderEntity order = nHibernateStorageContext.Session.Get<OrderEntity>(message.OrderId);
            order.Shipped = true;
            nHibernateStorageContext.Session.Update(order);
        }
    }

    #endregion
}