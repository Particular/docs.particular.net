using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler(ILogger<MessageWithLargePayloadHandler> logger) :
    IHandleMessages<MessageWithLargePayload>
{

    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        //ToDo check this
        logger.LogInformation($"Message received, size of blob property: {message.LargeBlob} Bytes");
        return Task.CompletedTask;
    }
}

#endregion
