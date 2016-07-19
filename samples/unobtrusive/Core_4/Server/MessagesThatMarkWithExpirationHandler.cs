using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MessagesThatMarkWithExpirationHandler :
    IHandleMessages<MessageThatExpires>
{
    static ILog log = LogManager.GetLogger(typeof(MessagesThatMarkWithExpirationHandler));

    public void Handle(MessageThatExpires message)
    {
        log.Info($"Message [{message.GetType()}] received, id: [{message.RequestId}]");
    }
}