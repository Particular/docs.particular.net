public class OrderShippingInformation
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public DateTimeOffset ShippedAt { get; set; }
}