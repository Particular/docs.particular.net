using NServiceBus;

public class OrderSubmitted :
    IEvent
{
    public string OrderId { get; set; }
    public decimal Value { get; set; }
    public string ShipTo { get; set; }
}