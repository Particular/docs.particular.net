using NServiceBus;
using System;

public class OrderLifecycleSagaData :
    ContainSagaData
{
    public virtual Guid OrderId { get; set; }
}