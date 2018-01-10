using NServiceBus;

public class MyEvent :
    IEvent
{
    public string Id { get; set; }
}