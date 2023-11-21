namespace Store.Messages.Events
{
    using NServiceBus;

    public class OrderCancelled :
        IEvent
    {
        public int OrderNumber { get; set; }
        public string ClientId { get; set; }
    }
}