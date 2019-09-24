using NServiceBus;

public class OtherMessage :
    IMessage
{
    public string Content { get; set; }
}