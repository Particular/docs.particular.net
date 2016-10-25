using NServiceBus;

namespace Messages.Events
{
    public class OrderBilled :
        IEvent
    {
        public string OrderId { get; set; }
    }
}