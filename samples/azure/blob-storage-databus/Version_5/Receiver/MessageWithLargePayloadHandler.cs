using NServiceBus;
using NServiceBus.Logging;

#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler : IHandleMessages<MessageWithLargePayload>
{
    static ILog log = LogManager.GetLogger<MessageWithLargePayloadHandler>();

    public void Handle(MessageWithLargePayload message)
    {
        log.InfoFormat("Message received. Description: '{0}'. Size of payload property: {1} Bytes", message.Description, message.LargePayload.Value.Length);
    }
}

#endregion