using NServiceBus;
using System;

class ShipOrder : IMessage
{
    public Guid OrderId { get; set;}
}
