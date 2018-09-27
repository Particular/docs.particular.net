using System;
using NServiceBus;

#region SomeEvent

public class SomeEvent :
    IEvent
{
    public Guid EventId { get; set; }
}

#endregion
