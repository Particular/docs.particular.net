using NServiceBus.Persistence.NHibernate;
using NServiceBus.Saga;

#region CustomSagaFinderNHibernate

class StartOrderSagaFinder :
    IFindSagas<OrderSagaData>.Using<StartOrder>
{
    NHibernateStorageContext storageContext;

    public StartOrderSagaFinder(NHibernateStorageContext storageContext)
    {
        this.storageContext = storageContext;
    }

    public OrderSagaData FindBy(StartOrder message)
    {
        var session = storageContext.Session;
        // if the instance is null a new saga will be automatically created if
        // the Saga handles the message as IAmStartedByMessages<StartOrder>; otherwise an exception is raised.
        return session.QueryOver<OrderSagaData>()
            .Where(d => d.OrderId == message.OrderId)
            .SingleOrDefault();
    }
}

#endregion

