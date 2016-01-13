using System;
using NServiceBus;

public class OrderLifecycleSagaData : IContainSagaData
{
    public virtual string OrderId { get; set; }
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }
}