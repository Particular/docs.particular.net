using NServiceBus;

public interface MyEvent :
    IEvent
{
    string Property { get; set; }
}