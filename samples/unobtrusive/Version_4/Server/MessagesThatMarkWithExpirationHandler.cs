using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MessagesThatMarkWithExpirationHandler : IHandleMessages<MessageThatExpires>
{
    static ILog log = LogManager.GetLogger(typeof(MessagesThatMarkWithExpirationHandler));

    public void Handle(MessageThatExpires message)
    {
        log.InfoFormat("Message [{0}] received, id: [{1}]", message.GetType(), message.RequestId);
    }
}