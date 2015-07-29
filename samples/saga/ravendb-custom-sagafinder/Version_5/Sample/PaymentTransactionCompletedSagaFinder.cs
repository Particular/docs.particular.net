using NServiceBus.RavenDB.Persistence;
using NServiceBus.Saga;
using Raven.Client;
using Raven.Client.UniqueConstraints;


class PaymentTransactionCompletedSagaFinder : IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
{
    ISessionProvider sessionProvider;

    public PaymentTransactionCompletedSagaFinder(ISessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public OrderSagaData FindBy(PaymentTransactionCompleted message)
    {
        IDocumentSession session = sessionProvider.Session;
        return session.LoadByUniqueConstraint<OrderSagaData>(d => d.PaymentTransactionId, message.PaymentTransactionId);
    }
}