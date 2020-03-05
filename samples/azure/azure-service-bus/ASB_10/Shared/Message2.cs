using NServiceBus;

public class Message2 :
    IMessage
{
    public string Property { get; set; }
}