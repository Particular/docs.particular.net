using NServiceBus;

namespace Core_9;

public class OrderPlaced :
    IEvent
{
    public string OrderId { get; set; }
}