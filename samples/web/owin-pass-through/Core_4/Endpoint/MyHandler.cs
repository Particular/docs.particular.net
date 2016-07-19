using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger(typeof(MyHandler));

    public void Handle(MyMessage message)
    {
        log.Info($"Received MyMessage. Property1:'{message.Property1}'. Property2:'{message.Property2}'");
    }
}