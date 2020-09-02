using System;
using NServiceBus;

public class OrderCompleted : IEvent
{
    public Guid OrderId { get; set; }
}