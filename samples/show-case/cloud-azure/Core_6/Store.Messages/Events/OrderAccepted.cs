namespace Store.Messages.Events
{
    using NServiceBus;

    public interface OrderAccepted :
        IEvent
    {
        int OrderNumber { get; set; }
        string[] ProductIds { get; set; }
        string ClientId { get; set; }
    }
}