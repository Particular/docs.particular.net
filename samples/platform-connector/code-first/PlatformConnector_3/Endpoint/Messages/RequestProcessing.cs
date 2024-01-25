using System;
using NServiceBus;

class RequestProcessing : IMessage
{
    public Guid BusinessId { get; set; }
}