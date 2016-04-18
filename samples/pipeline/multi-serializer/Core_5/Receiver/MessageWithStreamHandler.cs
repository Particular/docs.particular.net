using NServiceBus;
using NServiceBus.Logging;

public class MessageHandler :
    IHandleMessages<MessageWithJson>,
    IHandleMessages<MessageWithXml>
{
    static ILog log = LogManager.GetLogger<MessageHandler>();

    public void Handle(MessageWithJson message)
    {
        log.InfoFormat("Received JSON message with property '{0}'", message.SomeProperty);
    }
    public void Handle(MessageWithXml message)
    {
        log.InfoFormat("Received Xml message with property '{0}'", message.SomeProperty);
    }
}