using NServiceBus;

public class Message2 : IMessage
{
    public required string Property { get; set; }
}