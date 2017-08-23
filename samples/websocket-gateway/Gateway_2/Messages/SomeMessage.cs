using NServiceBus;

public class SomeMessage : IMessage
{
    public string Contents { get; set; }
}