using NServiceBus;

public class MessageExcluded :
    IMessage
{
    public string Property { get; set; }
}