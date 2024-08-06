using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MessageWithLargePayloadHandler : IHandleMessages<MessageWithLargePayload>
{
    static ILog log = LogManager.GetLogger<MessageWithLargePayloadHandler>();

    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        log.Info($"Message received. Description: '{message.Description}'. Size of payload property: {message.LargePayload.Value.Length} Bytes");
#pragma warning restore CS0618 // Type or member is obsolete
        return Task.CompletedTask;
    }
}