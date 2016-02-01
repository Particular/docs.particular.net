using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.RavenDB.Persistence;
using NServiceBus.Sagas;
using Raven.Client;
using Raven.Client.UniqueConstraints;


class PaymentTransactionCompletedSagaFinder : IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
{
    IAsyncSessionProvider sessionProvider;

    public PaymentTransactionCompletedSagaFinder(IAsyncSessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public Task<OrderSagaData> FindBy(PaymentTransactionCompleted message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        IAsyncDocumentSession session = sessionProvider.AsyncSession;
        return session.LoadByUniqueConstraintAsync<OrderSagaData>(d => d.PaymentTransactionId, message.PaymentTransactionId);
    }
}