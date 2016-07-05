using NServiceBus;
using NServiceBus.Logging;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger(typeof(MyHandler));

    public void Handle(MyMessage message)
    {
        log.Info("Hello from MyHandler");
    }
}