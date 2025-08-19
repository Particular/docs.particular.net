using System;
using NServiceBus;

public class NativeMessage : IMessage
{
    public Guid NativeMessageId { get; set; }
    public string Content { get; set; }
}