using System;

namespace Shared.Messages.In.A.Deep.Nested.Namespace.Nested.Events
{
    using NServiceBus;

    #region SomeEvent

    public class SomeEvent :
        IEvent
    {
        public Guid EventId { get; set; }
    }

    #endregion
}