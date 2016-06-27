using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class LargeMessagesHandler : IHandleMessages<LargeMessage>
{
    static ILog log = LogManager.GetLogger(typeof(LargeMessagesHandler));

    public void Handle(LargeMessage message)
    {
        if (message.LargeDataBus == null)
        {
            log.Info($"Message [{message.GetType()}] received, id:{message.RequestId}");
        }
        else
        {
            log.Info($"Message [{message.GetType()}] received, id:{message.RequestId} and payload {message.LargeDataBus.Length} bytes");
        }
    }
}