using NServiceBus;
using NServiceBus.Logging;

#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler : IHandleMessages<MessageWithLargePayload>
{
    static ILog log = LogManager.GetLogger(typeof(MessageWithLargePayloadHandler));

    public void Handle(MessageWithLargePayload message)
    {
        log.Info("Message received, size of blob property: " + message.LargeBlob.Value.Length + " Bytes");
    }
}

#endregion