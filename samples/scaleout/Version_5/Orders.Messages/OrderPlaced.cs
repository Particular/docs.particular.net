using NServiceBus;

namespace Orders.Events
{
    public class OrderPlaced :IEvent
    {
        public string OrderId { get; set; }
    }
}
