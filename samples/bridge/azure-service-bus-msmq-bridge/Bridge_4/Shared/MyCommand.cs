using NServiceBus;

public class MyCommand :
    IMessage
{
    public string Property { get; set; }
}