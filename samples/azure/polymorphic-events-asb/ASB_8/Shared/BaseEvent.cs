using System;

namespace Events
{
    using NServiceBus;

    #region BaseEvent

    public class BaseEvent :
        IEvent
    {
        public Guid EventId { get; set; }
    }

    #endregion
}
