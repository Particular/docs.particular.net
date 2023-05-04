namespace Messages;

public class OrderShipped : IMessage
{
  public string? OrderId { get; set; }
  public bool IsDuplicate { get; set; }
}
