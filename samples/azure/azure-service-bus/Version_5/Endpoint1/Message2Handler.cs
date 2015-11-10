using NServiceBus;
using NServiceBus.Logging;

public class Message2Handler : IHandleMessages<Message2>
{
    static ILog logger = LogManager.GetLogger<Message2Handler>();
    public void Handle(Message2 message)
    {
        logger.InfoFormat("Received Message2: {0}", message.Property);
    }
}