using NServiceBus;

public class IntMessage :
    IMessage
{
    public string Property { get; set; }
}