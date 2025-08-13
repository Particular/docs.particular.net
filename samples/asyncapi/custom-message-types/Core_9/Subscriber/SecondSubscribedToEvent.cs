using AsyncAPI.Feature;

#region SubscriberSecondEvent
[SubscribedEvent(EventName = "SomeNamespace.SecondEvent", Version = 1)]
public class SecondSubscribedToEvent
{
    public string SomeValue { get; init; }
}
#endregion