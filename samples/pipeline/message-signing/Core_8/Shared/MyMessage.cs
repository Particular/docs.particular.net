using NServiceBus;

public class MyMessage :
    IMessage
{
    public string Contents { get; set; }
}