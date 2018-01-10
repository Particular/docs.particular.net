using NServiceBus;

public class MyReply :
    IMessage
{
    public string Id { get; set; }
}