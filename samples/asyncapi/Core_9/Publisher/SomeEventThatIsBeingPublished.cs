using Infrastructure;

[PublishedEvent(EventName = "SomeNamespace.SomeEvent", Version = 1)]
public class SomeEventThatIsBeingPublished
{
    public string SomeValue { get; init; }
    public string SomeOtherValue { get; init; }
}