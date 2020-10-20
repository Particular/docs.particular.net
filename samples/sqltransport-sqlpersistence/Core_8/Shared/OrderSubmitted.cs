using System;
using NServiceBus;

public class OrderSubmitted :
    IEvent
{
    public Guid OrderId { get; set; }
    public int Value { get; set; }
}