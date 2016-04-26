﻿using NServiceBus.RavenDB.Persistence;
using NServiceBus.Saga;
using Raven.Client;
using Raven.Client.UniqueConstraints;

class CompleteOrderSagaFinder : IFindSagas<OrderSagaData>.Using<CompleteOrder>
{
    ISessionProvider sessionProvider;

    public CompleteOrderSagaFinder(ISessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public OrderSagaData FindBy(CompleteOrder message)
    {
        IDocumentSession session = sessionProvider.Session;
        return session.LoadByUniqueConstraint<OrderSagaData>(d => d.OrderId, message.OrderId);
    }
}
