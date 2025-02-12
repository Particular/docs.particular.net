using System;
using NServiceBus;

namespace Shared;

public class EventTwo : IEvent
{
    public required string Content { get; init; }
    public required DateTime PublishedOnUtc { get; init; }
}