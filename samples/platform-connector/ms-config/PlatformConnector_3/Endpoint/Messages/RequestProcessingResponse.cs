using System;
using NServiceBus;

class RequestProcessingResponse : IMessage
{
    public Guid BusinessId { get; set; }
}