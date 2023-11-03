namespace Events;

public class OrderDetails
{
    public Guid OrderId { get; set; }
    public IEnumerable<OrderLine> Lines { get; init; } = null!;
}