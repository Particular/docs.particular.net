using System;
using System.Threading.Tasks;
using NServiceBus;

#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler : IHandleMessages<MessageWithLargePayload>
{
    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message received. Description: '{0}'. Size of payload property: {1} Bytes", message.Description, message.LargePayload.Value.Length);
        return Task.FromResult(0);
    }
}

#endregion