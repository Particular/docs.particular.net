using NServiceBus;

public class NativeMessage : IMessage
{
    public string Content { get; set; }
}