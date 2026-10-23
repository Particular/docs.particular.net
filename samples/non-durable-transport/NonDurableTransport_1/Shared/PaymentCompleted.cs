using System;
using NServiceBus;

public class PaymentCompleted : IEvent
{
    public Guid OrderId { get; set; }
}
