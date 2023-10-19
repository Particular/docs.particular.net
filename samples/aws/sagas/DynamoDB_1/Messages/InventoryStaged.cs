using NServiceBus;

public class InventoryStaged : IEvent
{
  public string OrderId { get; set; }
}
