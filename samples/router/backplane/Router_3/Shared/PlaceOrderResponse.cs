using NServiceBus;

public class PlaceOrderResponse :
    IMessage
{
    public string OrderId { get; set; }
}