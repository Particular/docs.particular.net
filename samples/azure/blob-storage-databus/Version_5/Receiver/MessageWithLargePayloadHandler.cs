using System;
using NServiceBus;

#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler : IHandleMessages<MessageWithLargePayload>
{
    public void Handle(MessageWithLargePayload message)
    {
        Console.WriteLine("Message received. Description: '{0}'. Size of payload property: {1} Bytes", message.Description, message.LargePayload.Value.Length);
    }
}

#endregion