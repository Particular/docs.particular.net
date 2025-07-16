public class OrderSubmitted :
    IEvent
{
    public string OrderId { get; set; }
    public decimal Value { get; set; }
}