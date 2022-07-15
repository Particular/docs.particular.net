using NServiceBus;
using System;

class CreateOrder : IMessage
{
    public Guid OrderId { get; set; }
}