using System;
using NServiceBus;

public class StartOrder :
    IMessage, IProvideOrderId
{
    public Guid OrderId { get; set; }
}