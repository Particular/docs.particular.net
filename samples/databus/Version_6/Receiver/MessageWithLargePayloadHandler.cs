using System;
using System.Threading.Tasks;
using NServiceBus;

#region MessageWithLargePayloadHandler
public class MessageWithLargePayloadHandler : IHandleMessages<MessageWithLargePayload>
{
    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message received, size of blob property: " + message.LargeBlob.Value.Length + " Bytes");
        return Task.FromResult(0);
    }
}
#endregion