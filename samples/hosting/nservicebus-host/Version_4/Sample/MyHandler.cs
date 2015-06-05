using NServiceBus;
using NServiceBus.Logging;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger(typeof(MyHandler));

    public void Handle(MyMessage message)
    {
        logger.Info("Hello from MyHandler");
    }
}