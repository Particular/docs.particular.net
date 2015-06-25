using log4net;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger(typeof(MyHandler));

    public void Handle(MyMessage message)
    {
        logger.Error("Hello from MyHandler");
    }
}