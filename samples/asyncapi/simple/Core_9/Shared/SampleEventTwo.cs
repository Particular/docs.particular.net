using NServiceBus;
public class SampleEventTwo : IEvent
{
    public string SomeValue { get; init; }
}