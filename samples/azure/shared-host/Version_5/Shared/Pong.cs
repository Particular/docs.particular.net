using NServiceBus;

public class Pong : IMessage
{
    public string Message { get; set; }
}