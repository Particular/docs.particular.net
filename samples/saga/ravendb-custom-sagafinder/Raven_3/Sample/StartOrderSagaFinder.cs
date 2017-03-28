using NServiceBus.RavenDB.Persistence;
using NServiceBus.Saga;
using Raven.Client.UniqueConstraints;

#region CustomSagaFinderWithUniqueConstraintRavenDB

class StartOrderSagaFinder :
    IFindSagas<OrderSagaData>.Using<StartOrder>
{
    ISessionProvider sessionProvider;

    public StartOrderSagaFinder(ISessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public OrderSagaData FindBy(StartOrder message)
    {
        var session = sessionProvider.Session;
        // if the instance is null a new saga will be automatically created if
        // the Saga handles the message as IAmStartedByMessages<StartOrder>; otherwise an exception is raised.
        return session.LoadByUniqueConstraint<OrderSagaData>(d => d.OrderId, message.OrderId);
    }
}

#endregion