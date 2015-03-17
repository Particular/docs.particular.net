using System;
using NServiceBus;

namespace MyMessages
{
    public interface IMyEvent: IEvent
    {
        Guid EventId { get; set; }
        DateTime? Time { get; set; }
        TimeSpan Duration { get; set; }
    }
}
