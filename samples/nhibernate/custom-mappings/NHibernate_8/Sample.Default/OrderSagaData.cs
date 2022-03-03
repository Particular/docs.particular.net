using System;
using NServiceBus;
using NServiceBus.SagaPersisters.NHibernate;

public class OrderSagaData :
    ContainSagaData
{
    public virtual string OrderId { get; set; }
    [RowVersion]
    public virtual int Version { get; set; }
}

