using NServiceBus;

public class MyEvent : IEvent
{
    public byte[] Data { get; set; }
}