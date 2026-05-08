using NServiceBus;

namespace Messages;

public class OrderBilled : IEvent
{
    public string? OrderId { get; set; }
}