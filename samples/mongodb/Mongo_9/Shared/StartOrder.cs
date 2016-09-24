using System;
using NServiceBus;

public class StartOrder :
    IMessage
{
    public Guid OrderId { get; set; }
}