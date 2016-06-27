using NServiceBus;
using NServiceBus.Logging;

#region MessageWithLargePayloadHandler

public class MessageWithLargePayloadHandler : IHandleMessages<MessageWithLargePayload>
{
    static ILog log = LogManager.GetLogger<MessageWithLargePayloadHandler>();

    public void Handle(MessageWithLargePayload message)
    {
        log.Info($"Message received. Description: '{message.Description}'. Size of payload property: {message.LargePayload.Value.Length} Bytes");
    }
}

#endregion