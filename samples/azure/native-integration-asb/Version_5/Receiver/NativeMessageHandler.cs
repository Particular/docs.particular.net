using NServiceBus;
using NServiceBus.Logging;

#region NativeMessageHandler

public class NativeMessageHandler : IHandleMessages<NativeMessage>
{
    ILog logger = LogManager.GetLogger<NativeMessageHandler>();

    public void Handle(NativeMessage message)
    {
        logger.InfoFormat("Message content: {0}", message.Content);
        logger.InfoFormat("Received native message sent on {0} UTC", message.SendOnUtc);
    }

}

#endregion