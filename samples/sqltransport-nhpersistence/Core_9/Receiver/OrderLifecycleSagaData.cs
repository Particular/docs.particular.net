using System;
using NServiceBus;

public class OrderLifecycleSagaData :
    ContainSagaData
{
    public virtual Guid OrderId { get; set; }
}