using System;
using Messages;
using NServiceBus;

#region MessageWithLargePayloadHandler
public class MessageWithLargePayloadHandler : IHandleMessages<MessageWithLargePayload>
{
    public void Handle(MessageWithLargePayload message)
    {
        Console.WriteLine("Message received, size of blob property: " + message.LargeBlob.Value.Length + " Bytes");
    }
}
#endregion