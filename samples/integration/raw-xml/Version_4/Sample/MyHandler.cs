using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler : IHandleMessages<MessageWithXDocument>, IHandleMessages<MessageWithXElement>
{
    static ILog logger = LogManager.GetLogger(typeof(MyHandler));

    public void Handle(MessageWithXDocument message)
    {
        logger.InfoFormat("Document: {0}", message.codes.ToString());
    }

    public void Handle(MessageWithXElement message)
    {
        logger.InfoFormat("Element: {0}", message.codes.ToString());
    }
}