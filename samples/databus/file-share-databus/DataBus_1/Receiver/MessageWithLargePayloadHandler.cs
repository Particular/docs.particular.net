using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler(ILogger<MessageWithLargePayloadHandler> logger) :
    IHandleMessages<MessageWithLargePayload>
{

    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Message received, size of blob property: {message.LargeBlob.Value.Length} Bytes");
        return Task.CompletedTask;
    }
}

#endregion