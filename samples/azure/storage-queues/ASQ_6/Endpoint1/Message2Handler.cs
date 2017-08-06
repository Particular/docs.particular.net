using NServiceBus;
using NServiceBus.Logging;

public class Message2Handler :
    IHandleMessages<Message2>
{
    static ILog log = LogManager.GetLogger<Message2Handler>();

    public void Handle(Message2 message)
    {
        log.Info($"Received Message2: {message.Property}");
    }
}