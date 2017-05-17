using NServiceBus.RavenDB.Persistence;
using NServiceBus.Saga;
using Raven.Client.UniqueConstraints;


#region finder
class CompletePaymentTransactionSagaFinder :
    IFindSagas<OrderSagaData>.Using<CompletePaymentTransaction>
{
    ISessionProvider sessionProvider;

    public CompletePaymentTransactionSagaFinder(ISessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public OrderSagaData FindBy(CompletePaymentTransaction message)
    {
        var session = sessionProvider.Session;
        // if the instance is null a new saga will be automatically created if
        // the Saga handles the message as IAmStartedByMessages<CompletePaymentTransaction>
        // otherwise an exception is raised.
        return session.LoadByUniqueConstraint<OrderSagaData>(d => d.PaymentTransactionId, message.PaymentTransactionId);
    }
}
#endregion