namespace Store.Messages.Events
{
    using NServiceBus;

    public class OrderPlaced :
        IEvent
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
    }
}