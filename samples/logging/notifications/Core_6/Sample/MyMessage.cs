using NServiceBus;

public class MyMessage :
    IMessage
{
    public string Property { get; set; }
}