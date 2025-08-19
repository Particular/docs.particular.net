using NServiceBus;

public class StartOrder :
    IMessage
{
    public string OrderId { get; set; }
}