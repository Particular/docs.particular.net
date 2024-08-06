using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#pragma warning disable CS0618 // Type or member is obsolete
#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler :
    IHandleMessages<MessageWithLargePayload>
{
    static ILog log = LogManager.GetLogger<MessageWithLargePayloadHandler>();

    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        log.Info($"Message received. Description: '{message.Description}'. Size of payload property: {message.LargePayload.Value.Length} Bytes");
        return Task.CompletedTask;
    }
}

#endregion
#pragma warning restore CS0618 // Type or member is obsolete