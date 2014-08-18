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
        var order = nHibernateStorageContext.Session.Get<OrderEntity>(message.OrderId);
        order.Shipped = true;
        nHibernateStorageContext.Session.Update(order);
    }
}

#endregion

class OrderEntity
{
    public bool Shipped;
}
class OrderPlaced
{
    public object OrderId { get; set; }
}