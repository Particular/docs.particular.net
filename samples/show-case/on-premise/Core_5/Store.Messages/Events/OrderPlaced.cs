namespace Store.Messages.Events
{
    using NServiceBus;

    public interface OrderPlaced : IEvent
    {
        int OrderNumber { get; set; }
        string[] ProductIds { get; set; }
        string ClientId { get; set; }
    }
}