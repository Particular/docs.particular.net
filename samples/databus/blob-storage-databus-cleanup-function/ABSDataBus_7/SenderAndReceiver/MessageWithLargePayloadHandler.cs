using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MessageWithLargePayloadHandler(ILogger<MessageWithLargePayloadHandler> logger) : IHandleMessages<MessageWithLargePayload>
{

    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        logger.LogInformation("Message received. Description: '{Description}'. Size of payload property: {PayloadSize} Bytes", message.Description, message.LargePayload.Value.Length);
        #pragma warning restore CS0618 // Type or member is obsolete
        return Task.CompletedTask;
    }
}