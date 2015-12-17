using NServiceBus;

public class PlaceOrder:IMessage
{
    public string OrderNumber { get; set; }
    public double OrderValue { get; set; }
}