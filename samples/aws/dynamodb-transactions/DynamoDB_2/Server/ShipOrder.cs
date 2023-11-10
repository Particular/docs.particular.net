using System;
using NServiceBus;

public class ShipOrder :
    IMessage
{
    public Guid OrderId { get; set; }
}