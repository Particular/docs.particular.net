namespace Store.Messages.Events
{
    //NServiceBus messages can be defined using both classes and interfaces
    public interface OrderAccepted 
    {
        int OrderNumber { get; set; }
        string[] ProductIds { get; set; }
        string ClientId { get; set; }
    }
}