using System;
using NServiceBus;

public class OrderReceived :
    IMessage
{
    public Guid OrderId { get; set; }
}