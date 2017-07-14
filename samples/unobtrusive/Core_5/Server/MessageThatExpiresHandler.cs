using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MessageThatExpiresHandler :
    IHandleMessages<MessageThatExpires>
{
    static ILog log = LogManager.GetLogger<MessageThatExpiresHandler>();

    public void Handle(MessageThatExpires message)
    {
        log.Info($"Message [{message.GetType()}] received, id: [{message.RequestId}]");
    }
}