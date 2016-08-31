using System;
using NServiceBus;

public class LongProcessingRequestReply : IMessage
{
    public Guid Id { get; set; }
}