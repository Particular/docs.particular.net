using NServiceBus;

public class Event2 :
    IEvent
{
    public string Property { get; set; }
    public long EventNumber { get; set; }
}