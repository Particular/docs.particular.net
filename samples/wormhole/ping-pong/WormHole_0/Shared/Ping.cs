using NServiceBus;

public class Ping : IMessage
{
    public string Payload { get; set; }
}