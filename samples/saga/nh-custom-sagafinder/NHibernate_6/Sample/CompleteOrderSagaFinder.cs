using NServiceBus.Persistence.NHibernate;
using NServiceBus.Saga;

class CompleteOrderSagaFinder : IFindSagas<OrderSagaData>.Using<CompleteOrder>
{
    NHibernateStorageContext storageContext;

    public CompleteOrderSagaFinder(NHibernateStorageContext storageContext)
    {
        this.storageContext = storageContext;
    }

    public OrderSagaData FindBy(CompleteOrder message)
    {
        var session = storageContext.Session;
        return session.QueryOver<OrderSagaData>()
            .Where(d => d.OrderId == message.OrderId)
            .SingleOrDefault();
    }
}