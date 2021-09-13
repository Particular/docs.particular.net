using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

#region finder
class CompletePaymentTransactionSagaFinder :
    ISagaFinder<OrderSagaData, CompletePaymentTransaction>
{
    public Task<OrderSagaData> FindBy(CompletePaymentTransaction message, ISynchronizedStorageSession storageSession,
        IReadOnlyContextBag context, CancellationToken cancellationToken = new CancellationToken())
    {
        var session = storageSession.Session();
        var orderSagaData = session.QueryOver<OrderSagaData>()
            .Where(d => d.PaymentTransactionId == message.PaymentTransactionId)
            .SingleOrDefault();
        return Task.FromResult(orderSagaData);
    }
}
#endregion