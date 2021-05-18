using NServiceBus;

public interface OtherEvent :
    IEvent
{
    string Property { get; set; }
}