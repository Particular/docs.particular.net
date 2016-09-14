using System;
using NServiceBus;

public class LongProcessingRequest :
    IMessage
{
    public Guid Id { get; set; }
    public TimeSpan EstimatedProcessingTime { get; set; }
}