using System;
using NServiceBus;

public class CompleteOrder :
    IMessage
{
    public Guid OrderId { get; set; }
}