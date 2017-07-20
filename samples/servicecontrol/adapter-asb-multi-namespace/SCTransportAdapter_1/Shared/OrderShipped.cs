using NServiceBus;

public class OrderShipped :
    IMessage
{
    public string OrderId { get; set; }
    public decimal Value { get; set; }
}