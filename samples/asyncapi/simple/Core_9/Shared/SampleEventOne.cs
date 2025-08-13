using NServiceBus;
public class SampleEventOne : IEvent
{
    public string SomeValue { get; init; }
}