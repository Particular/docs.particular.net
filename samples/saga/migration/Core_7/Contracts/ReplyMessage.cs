using NServiceBus;

public class ReplyMessage :
    IMessage
{
    public string SomeId { get; set; }
}