namespace Messages;

public class OrderShipped : IEvent
{
  public string? OrderId { get; set; }
  public bool IsDuplicate { get; set; }
}
