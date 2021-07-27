using System;
using NServiceBus;

public class OrderCompleted :
    IEvent, IProvideOrderId
{
    public Guid OrderId { get; set; }
}