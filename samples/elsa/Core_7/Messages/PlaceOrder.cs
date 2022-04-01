using NServiceBus;

namespace Messages
{
  public class PlaceOrder : ICommand
  {
    public PlaceOrder(string orderId)
    {
      OrderId = orderId;
    }

    public string OrderId { get; set; }
  }
}