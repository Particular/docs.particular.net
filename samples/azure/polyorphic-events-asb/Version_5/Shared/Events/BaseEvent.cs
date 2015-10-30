using System;

namespace Events
{
    using NServiceBus;

    public class BaseEvent : IEvent
    {
        public Guid EventId { get; set; }
    }
}
