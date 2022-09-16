using NServiceBus;

public class MyMessage : IMessage
{
    public string EntityId { get; set; }
}