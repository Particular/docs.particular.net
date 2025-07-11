using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler(ILogger<MessageWithLargePayloadHandler> logger) :
    IHandleMessages<MessageWithLargePayload>
{

    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received, size of blob property: {BlobSize} Bytes", message.LargeBlob.Value.Length);
        return Task.CompletedTask;
    }
}

#endregion