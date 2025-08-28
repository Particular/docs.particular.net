using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MessageWithLargePayloadHandler(ILogger<MessageWithLargePayloadHandler> logger)
    : IHandleMessages<MessageWithLargePayload>
{
    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        logger.LogInformation(
            "Message received. Description: '{Description}'. Size of payload property: {PayloadSize} Bytes",
            message.Description, message.LargePayload.Value.Length);

        return Task.CompletedTask;
    }
}