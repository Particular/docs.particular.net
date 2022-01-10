using NServiceBus;

namespace Messages
{
    public class OrderBilled : IEvent
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
        public decimal OrderValue { get; set; }
    }
}