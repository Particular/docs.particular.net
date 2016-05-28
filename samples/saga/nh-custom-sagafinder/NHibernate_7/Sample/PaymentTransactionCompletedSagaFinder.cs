using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

class PaymentTransactionCompletedSagaFinder : IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
{

    public Task<OrderSagaData> FindBy(PaymentTransactionCompleted message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        var session = storageSession.Session();
        var orderSagaData = session.QueryOver<OrderSagaData>()
            .Where(d => d.PaymentTransactionId == message.PaymentTransactionId)
            .SingleOrDefault();
        return Task.FromResult(orderSagaData);
    }

}