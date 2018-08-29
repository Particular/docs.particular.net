using NServiceBus;

public class MyMessage : IMessage
{
    public string Content { get; set; }
}