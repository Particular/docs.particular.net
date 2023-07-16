using NServiceBus;

namespace Core_8
{
    public class OrderPlaced :
        IEvent
    {
        public string OrderId { get; set; }
    }
}