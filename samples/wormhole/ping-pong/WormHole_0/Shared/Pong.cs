using NServiceBus;

public class Pong : IMessage
{
    public string Payload { get; set; }
}