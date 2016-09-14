using System;
using NServiceBus;
public class LongProcessingFinished :
    IEvent
{
    public Guid Id { get; set; }
}