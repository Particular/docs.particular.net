using NServiceBus;
using NServiceBus.Logging;

#region NativeMessageHandler

public class NativeMessageHandler :
    IHandleMessages<NativeMessage>
{
    static ILog log = LogManager.GetLogger<NativeMessageHandler>();

    public void Handle(NativeMessage message)
    {
        log.Info($"Message content: {message.Content}");
        log.Info($"Received native message sent on {message.SendOnUtc} UTC");
    }
}

#endregion