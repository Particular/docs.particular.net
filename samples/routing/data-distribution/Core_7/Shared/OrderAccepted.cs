using NServiceBus;

public class OrderAccepted :
    IEvent
{
    public string OrderId { get; set; }
}