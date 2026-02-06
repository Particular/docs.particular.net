using NServiceBus;

public class ExcludedMessage : IMessage
{
    public string Property { get; set; }
}