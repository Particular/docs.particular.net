using NServiceBus.Persistence.NHibernate;
using NServiceBus.Saga;

#region finder
class CompletePaymentTransactionSagaFinder :
    IFindSagas<OrderSagaData>.Using<CompletePaymentTransaction>
{
    NHibernateStorageContext storageContext;

    public CompletePaymentTransactionSagaFinder(NHibernateStorageContext storageContext)
    {
        this.storageContext = storageContext;
    }

    public OrderSagaData FindBy(CompletePaymentTransaction message)
    {
        var session = storageContext.Session;
        return session.QueryOver<OrderSagaData>()
            .Where(d => d.PaymentTransactionId == message.PaymentTransactionId)
            .SingleOrDefault();
    }
}
#endregion