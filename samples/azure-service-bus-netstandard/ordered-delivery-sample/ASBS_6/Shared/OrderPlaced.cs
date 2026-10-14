using NServiceBus;

namespace Shared;

public class OrderPlaced : IEvent
{
    public Guid OrderId { get; set; }
}