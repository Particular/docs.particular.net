using NServiceBus.Persistence.NHibernate;
using NServiceBus.Saga;
using NHibernate;

class CompleteOrderSagaFinder : IFindSagas<OrderSagaData>.Using<CompleteOrder>
{
    NHibernateStorageContext storageContext;

    public CompleteOrderSagaFinder(NHibernateStorageContext storageContext)
    {
        this.storageContext = storageContext;
    }

    public OrderSagaData FindBy(CompleteOrder message)
    {
        ISession session = storageContext.Session;
        return session.QueryOver<OrderSagaData>()
            .Where(d => d.OrderId == message.OrderId)
            .SingleOrDefault();
    }
}