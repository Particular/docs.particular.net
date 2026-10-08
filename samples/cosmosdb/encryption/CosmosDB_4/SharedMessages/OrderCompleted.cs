public class OrderCompleted :
    IEvent
{
    public Guid OrderId { get; set; }
}