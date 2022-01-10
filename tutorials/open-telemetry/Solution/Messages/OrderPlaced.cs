using NServiceBus;

namespace Messages
{
    public class OrderPlaced : IEvent
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
    }
}