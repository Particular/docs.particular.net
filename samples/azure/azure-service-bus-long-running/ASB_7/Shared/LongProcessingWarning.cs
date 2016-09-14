using System;
using NServiceBus;
public class LongProcessingWarning :
    IEvent
{
    public Guid Id { get; set; }
}