public class OrderShipped : IEvent
{
    public Guid OrderId { get; set; }
    public DateTimeOffset ShippingDate { get; set; }
}