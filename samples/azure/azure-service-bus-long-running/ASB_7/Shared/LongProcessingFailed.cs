using System;
using NServiceBus;
public class LongProcessingFailed :
    IEvent
{
    public Guid Id { get; set; }
    public string Reason { get; set; }
}