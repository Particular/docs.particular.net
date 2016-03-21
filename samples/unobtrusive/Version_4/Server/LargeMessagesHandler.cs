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
            log.InfoFormat("Message [{0}] received, id:{1}", message.GetType(), message.RequestId);
        }
        else
        {
            log.InfoFormat("Message [{0}] received, id:{1} and payload {2} bytes", message.GetType(), message.RequestId, message.LargeDataBus.Length);
        }
    }
}