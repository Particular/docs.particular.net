using Events;
using NServiceBus;
using NServiceBus.Logging;

public class MyEventHandler :
    IHandleMessages<IMyEvent>
{
    static ILog log = LogManager.GetLogger(typeof(MyEventHandler));

    public void Handle(IMyEvent message)
    {
        log.Info($"IMyEvent received from server with id:{message.EventId}");
    }
}