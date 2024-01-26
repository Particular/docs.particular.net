using System;
using NServiceBus;

class BusinessMessage : IMessage
{
    public Guid BusinessId { get; set; }
}