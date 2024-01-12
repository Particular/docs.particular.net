using NServiceBus;
using System;

class BillOrder : IMessage
{
    public Guid OrderId { get; set;}
}