using NServiceBus;

public class OrderReceived : IEvent
{
  public string OrderId { get; set; }
}
