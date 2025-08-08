using Infrastructure;

[SubscribedEvent(EventName = "SomeNamespace.SecondEvent", Version = 1)]
public class SecondEvent
{
    public string SomeValue { get; init; }
    public string SomeOtherValue { get; init; }
}