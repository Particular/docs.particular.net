namespace Shared;

using NServiceBus;

public class OrderReceived : IEvent
{
    public Guid OrderId { get; set; }
}