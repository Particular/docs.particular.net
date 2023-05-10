using NServiceBus;

namespace Messages
{
  public class OrderShipped : IEvent
  {
    public string? OrderId { get; set; }
  }
}