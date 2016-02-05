using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;
using Raven.Client;
using Raven.Client.UniqueConstraints;

class PaymentTransactionCompletedSagaFinder : IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
{
    public Task<OrderSagaData> FindBy(PaymentTransactionCompleted message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        IAsyncDocumentSession session = storageSession.Session();
        return session.LoadByUniqueConstraintAsync<OrderSagaData>(d => d.PaymentTransactionId, message.PaymentTransactionId);
    }
}