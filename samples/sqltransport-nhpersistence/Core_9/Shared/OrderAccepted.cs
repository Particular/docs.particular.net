using System;
using NServiceBus;

public class OrderAccepted :
    IMessage
{
    public Guid OrderId { get; set; }
}