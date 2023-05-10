using NServiceBus;

namespace Messages
{
  public class OrderCancelled : IEvent
  {
    public string? OrderId { get; set; }
  }
}