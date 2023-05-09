namespace Messages;

public class OrderCancelled : IEvent
{
  public string? OrderId { get; set; }

  public bool IsCancelled { get; set; }
}
