using NServiceBus;

public class MyMessage :
    IMessage
{
    public string Id { get; set; }
}