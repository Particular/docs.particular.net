using System;
using NServiceBus;
using NServiceBus.SagaPersisters.NHibernate;

#pragma warning disable NSB0012 // Saga data classes should inherit ContainSagaData - Required for [RowVersion]

public class OrderSagaData :
    IContainSagaData
{
    public virtual string OrderId { get; set; }
    [RowVersion]
    public virtual int Version { get; set; }
    public virtual string OriginalMessageId { get; set; }
    public virtual string Originator { get; set; }
    public virtual Guid Id { get; set; }
}

#pragma warning restore NSB0012 // Saga data classes should inherit ContainSagaData