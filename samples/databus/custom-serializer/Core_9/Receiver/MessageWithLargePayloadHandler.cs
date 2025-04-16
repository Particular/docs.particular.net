using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

public class MessageWithLargePayloadHandler(ILogger<MessageWithLargePayloadHandler> logger) :
    IHandleMessages<MessageWithLargePayload>
{

    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        logger.LogInformation($"Message received containing {message.LargeData.Value.Length} measurements");
#pragma warning restore CS0618 // Type or member is obsolete
        return Task.CompletedTask;
    }
}