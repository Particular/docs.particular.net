using System;
using NServiceBus;

public class OrderShipped : IMessage
{
    public Guid OrderId { get; set; }
    public DateTimeOffset ShippingDate { get; set; }
}