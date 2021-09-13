using NServiceBus;

public class SomeMessage : IMessage
{
    public int Counter { get; set; }
}
