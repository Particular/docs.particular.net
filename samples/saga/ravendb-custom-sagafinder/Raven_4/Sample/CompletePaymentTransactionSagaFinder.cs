using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;
using Raven.Client.UniqueConstraints;

#region finder
class CompletePaymentTransactionSagaFinder :
    IFindSagas<OrderSagaData>.Using<CompletePaymentTransaction>
{
    public Task<OrderSagaData> FindBy(CompletePaymentTransaction message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        var session = storageSession.RavenSession();
        return session.LoadByUniqueConstraintAsync<OrderSagaData>(d => d.PaymentTransactionId, message.PaymentTransactionId);
    }
}
#endregion