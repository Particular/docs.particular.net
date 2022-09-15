using NServiceBus;

namespace Messages
{
    public class OrderPlaced :
        IEvent
    {
        public OrderPlaced(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
    }
}
