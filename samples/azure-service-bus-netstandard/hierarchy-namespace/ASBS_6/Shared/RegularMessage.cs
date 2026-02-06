using NServiceBus;

public class RegularMessage : IMessage
{
    public string Property { get; set; }
}