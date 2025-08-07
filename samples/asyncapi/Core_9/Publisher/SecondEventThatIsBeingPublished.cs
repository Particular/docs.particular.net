using Infrastructure;

[PublishedEvent(EventName = "SomeNamespace.SecondEvent", Version = 1)]
public class SecondEventThatIsBeingPublished
{
    public string SomeValue { get; init; }
    public string SomeOtherValue { get; init; }
}