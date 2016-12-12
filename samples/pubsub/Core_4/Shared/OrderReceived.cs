using System;
using NServiceBus;

public interface OrderReceived : IEvent
{
    Guid OrderId { get; set; }
}