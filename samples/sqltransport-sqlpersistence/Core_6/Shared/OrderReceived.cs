using NServiceBus;

public class OrderReceived :
    IMessage
{
    public string OrderId { get; set; }
}