using NServiceBus;

namespace Messages
{
    public class OrderBilled :
        IEvent
    {
        public OrderBilled(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
    }
}