using NServiceBus;

namespace Messages.Events
{
    public class OrderPlaced :
        IEvent
    {
        public string OrderId { get; set; }
    }
}
