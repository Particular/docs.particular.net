using NServiceBus;

namespace Messages
{
  public class PlaceOrder : IMessage
  {
    public string? OrderId { get; set; }
  }
}