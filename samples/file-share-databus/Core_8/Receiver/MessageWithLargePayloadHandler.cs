using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler :
    IHandleMessages<MessageWithLargePayload>
{
    static ILog log = LogManager.GetLogger<MessageWithLargePayloadHandler>();

    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        log.Info($"Message received, size of blob property: {message.LargeBlob.Value.Length} Bytes");
        return Task.CompletedTask;
    }
}

#endregion