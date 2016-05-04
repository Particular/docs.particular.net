using System;

namespace Shared.Messages.In.A.Deep.Nested.Namespace.Nested.Events
{
    using NServiceBus;

    #region SuperDuperEvent

    public class SuperDuperEvent : IEvent
    {
        public Guid EventId { get; set; }
    }

    #endregion
}