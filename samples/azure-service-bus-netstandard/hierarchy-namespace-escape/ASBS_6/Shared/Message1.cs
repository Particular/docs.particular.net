using NServiceBus;

public class Message1 :
    IMessage
{
    public string Property { get; set; }
}