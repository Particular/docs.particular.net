using NServiceBus;

public class OrderSubmitted :
    IMessage
{
    public string OrderId { get; set; }
    public decimal Value { get; set; }
    public string ShipTo { get; set; }
}