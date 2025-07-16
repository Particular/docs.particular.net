using System;
using NServiceBus;

namespace SharedMessages;

public record OrderCompleted : IEvent
{
    public required Guid OrderId { get; set; }
}