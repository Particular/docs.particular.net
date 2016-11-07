using NServiceBus;

namespace Core_6
{
    public class OrderPlaced :
        IEvent
    {
        public string OrderId { get; set; }
    }
}