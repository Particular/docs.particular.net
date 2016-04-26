using System.Threading.Tasks;
using NHibernate;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

class PaymentTransactionCompletedSagaFinder : IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
{

    public Task<OrderSagaData> FindBy(PaymentTransactionCompleted message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        ISession session = storageSession.Session();
        OrderSagaData orderSagaData = session.QueryOver<OrderSagaData>()
            .Where(d => d.PaymentTransactionId == message.PaymentTransactionId)
            .SingleOrDefault();
        return Task.FromResult(orderSagaData);
    }

}