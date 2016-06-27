using NServiceBus;
using NServiceBus.Logging;

public class MessageHandler :
    IHandleMessages<MessageWithJson>,
    IHandleMessages<MessageWithXml>
{
    static ILog log = LogManager.GetLogger<MessageHandler>();

    public void Handle(MessageWithJson message)
    {
        log.Info($"Received JSON message with property '{message.SomeProperty}'");
    }
    public void Handle(MessageWithXml message)
    {
        log.Info($"Received Xml message with property '{message.SomeProperty}'");
    }
}