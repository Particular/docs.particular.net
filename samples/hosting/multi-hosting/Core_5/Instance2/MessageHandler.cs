using NServiceBus;
using NServiceBus.Logging;

public class MessageHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MessageHandler>();

    public void Handle(MyMessage message)
    {
        log.Info("Hello from Instance 2");
    }
}