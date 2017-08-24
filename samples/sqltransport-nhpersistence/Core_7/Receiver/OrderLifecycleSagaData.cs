using System;
using NServiceBus;

public class OrderLifecycleSagaData :
    IContainSagaData
{
    public virtual Guid OrderId { get; set; }
    public virtual Guid Id { get; set; }
    public virtual string Originator { get; set; }
    public virtual string OriginalMessageId { get; set; }
}