using NServiceBus;

namespace Messages
{
  public class InventoryStaged : IEvent
  {
    public string? OrderId { get; set; }
  }
}