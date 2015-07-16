using System;
using NServiceBus.Saga;

public class OrderSagaDataFluent : IContainSagaData
{
    public virtual Guid Id { get; set; }
    public virtual string OriginalMessageId { get; set; }
    public virtual string Originator { get; set; }
    public virtual string OrderId { get; set; }
    public virtual int Version { get; set; }
}