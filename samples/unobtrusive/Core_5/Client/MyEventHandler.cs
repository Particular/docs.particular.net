using Events;
using NServiceBus;
using NServiceBus.Logging;

public class MyEventHandler :
    IHandleMessages<MyEvent>
{
    static ILog log = LogManager.GetLogger<MyEventHandler>();

    public void Handle(MyEvent message)
    {
        log.Info($"IMyEvent received from server with id:{message.EventId}");
    }
}