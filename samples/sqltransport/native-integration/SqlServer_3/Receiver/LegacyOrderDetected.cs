using NServiceBus;

public class LegacyOrderDetected :
    IMessage
{
    public string OrderId { get; set; }
}