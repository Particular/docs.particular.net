using NServiceBus;

public class MyEvent : IEvent
{
    public string CorrelationID { get; set; }
}