using AsyncAPI.Feature;

#region SubscriberFirstEvent
[SubscribedEvent(EventName = "SomeNamespace.FirstEvent", Version = 1)]
public class FirstSubscribedToEvent
{
    public string SomeOtherValue { get; init; }
}
#endregion