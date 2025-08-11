using AsyncAPI.Feature;

[PublishedEvent(EventName = "SomeNamespace.FirstEvent", Version = 1)]
public class FirstEventThatIsBeingPublished
{
    public string SomeValue { get; init; }
    public string SomeOtherValue { get; init; }
}