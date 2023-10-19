using NServiceBus;

public class CustomerBilled : IEvent
{
  public string OrderId { get; set; }
}
