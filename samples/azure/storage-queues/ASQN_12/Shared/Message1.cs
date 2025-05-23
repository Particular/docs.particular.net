using NServiceBus;

public class MyRequest :
    IMessage
{
    public string Property { get; set; }
}