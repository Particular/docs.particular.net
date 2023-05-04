namespace Messages;

public class ShipOrder : ICommand
{
  public string? OrderId { get; set; }

  public bool IsDuplicate { get; set; }
}
