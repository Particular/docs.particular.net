using System;
using NServiceBus;

public class LongProcessingReply :
    IMessage
{
    public Guid Id { get; set; }
}