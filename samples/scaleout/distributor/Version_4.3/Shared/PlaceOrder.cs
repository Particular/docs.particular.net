using System;
using NServiceBus;

public class PlaceOrder : IMessage
{
    public Guid OrderId { get; set; }
}