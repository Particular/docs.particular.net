using NServiceBus;

namespace Messages
{
  public class OrderReceived : IEvent
  {
    public string? OrderId { get; set; }
  }
}