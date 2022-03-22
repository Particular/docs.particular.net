using NServiceBus;

public class OrderAccepted :
    IMessage
{
    public string OrderId { get; set; }
}