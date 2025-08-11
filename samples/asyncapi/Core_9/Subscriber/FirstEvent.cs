using AsyncAPI.Feature;

[SubscribedEvent(EventName = "SomeNamespace.FirstEvent", Version = 1)]
public class FirstEvent
{
    public string SomeOtherValue { get; init; }
}