using NServiceBus;

public class Pong : IEvent
{
    public string Payload { get; set; }
}