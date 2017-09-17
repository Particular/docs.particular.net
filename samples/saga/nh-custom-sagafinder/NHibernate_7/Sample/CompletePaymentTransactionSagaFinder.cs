using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

#region finder
class CompletePaymentTransactionSagaFinder :
    IFindSagas<OrderSagaData>.Using<CompletePaymentTransaction>
{

    public Task<OrderSagaData> FindBy(CompletePaymentTransaction message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        var session = storageSession.Session();
        var orderSagaData = session.QueryOver<OrderSagaData>()
            .Where(d => d.PaymentTransactionId == message.PaymentTransactionId)
            .SingleOrDefault();
        return Task.FromResult(orderSagaData);
    }
}
#endregion