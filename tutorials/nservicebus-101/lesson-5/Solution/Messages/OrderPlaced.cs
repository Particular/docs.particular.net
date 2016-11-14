using NServiceBus;

namespace Messages
{
    public class OrderPlaced :
        IEvent
    {
        public string OrderId { get; set; }
    }
}