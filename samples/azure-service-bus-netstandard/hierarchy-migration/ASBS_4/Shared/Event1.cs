using NServiceBus;

public class Event1 :
    IEvent
{
    public string Property { get; set; }
    public long EventNumber { get; set; }
}