using NServiceBus;

public class ReplyMessage :
    IMessage
{
    public string Property { get; set; }
}