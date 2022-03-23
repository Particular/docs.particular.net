using System;
using NServiceBus;

public class OrderLifecycleSagaData :
#pragma warning disable NSB0012 // Saga data classes should inherit ContainSagaData
    IContainSagaData
#pragma warning restore NSB0012 // Saga data classes should inherit ContainSagaData
{
    public virtual Guid OrderId { get; set; }
    public virtual Guid Id { get; set; }
    public virtual string Originator { get; set; }
    public virtual string OriginalMessageId { get; set; }
}