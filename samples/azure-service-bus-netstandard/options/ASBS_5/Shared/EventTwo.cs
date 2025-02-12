using System;
using NServiceBus;

namespace Shared;

public class EventTwo : IEvent
{
    public string Content { get; set; }
    public DateTime PublishedOnUtc { get; set; }
}