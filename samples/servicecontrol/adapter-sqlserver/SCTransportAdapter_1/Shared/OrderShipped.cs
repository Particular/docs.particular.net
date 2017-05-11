using NServiceBus;

public class OrderShipped :
    IEvent
{
    public string OrderId { get; set; }
    public decimal Value { get; set; }
}