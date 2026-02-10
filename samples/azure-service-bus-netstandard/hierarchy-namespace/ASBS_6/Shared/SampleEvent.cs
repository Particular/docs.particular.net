using System;
using NServiceBus;

namespace Shared;

public class SampleEvent : IEvent
{
    public string Property { get; set; }
}