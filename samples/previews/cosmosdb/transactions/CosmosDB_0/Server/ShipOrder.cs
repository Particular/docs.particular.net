using System;
using NServiceBus;

public class ShipOrder :
    IMessage, IProvideOrderId
{
    public Guid OrderId { get; set; }
}