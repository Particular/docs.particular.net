using System;
using NServiceBus;
using NServiceBus.SagaPersisters.NHibernate;

public class OrderSagaData :
    IContainSagaData
{
    public virtual Guid Id { get; set; }
    public virtual string OriginalMessageId { get; set; }
    public virtual string Originator { get; set; }
    public virtual string OrderId { get; set; }
    [RowVersion]
    public virtual int Version { get; set; }
}

