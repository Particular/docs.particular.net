using NServiceBus;

public class MyReplyMessage :
    IMessage
{
    public string Content { get; set; }
}