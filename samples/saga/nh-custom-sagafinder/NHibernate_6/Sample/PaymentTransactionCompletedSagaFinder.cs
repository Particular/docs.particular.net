using NServiceBus.Persistence.NHibernate;
using NServiceBus.Saga;
using NHibernate;

class PaymentTransactionCompletedSagaFinder : IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
{
    NHibernateStorageContext storageContext;

    public PaymentTransactionCompletedSagaFinder(NHibernateStorageContext storageContext)
    {
        this.storageContext = storageContext;
    }

    public OrderSagaData FindBy(PaymentTransactionCompleted message)
    {
        ISession session = storageContext.Session;
        return session.QueryOver<OrderSagaData>()
            .Where(d => d.PaymentTransactionId == message.PaymentTransactionId)
            .SingleOrDefault();
    }
}