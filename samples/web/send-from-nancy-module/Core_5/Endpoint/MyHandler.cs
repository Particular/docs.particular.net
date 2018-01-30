using NServiceBus;
using NServiceBus.Logging;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public void Handle(MyMessage message)
    {
        log.Info("Message received at endpoint");
    }
}