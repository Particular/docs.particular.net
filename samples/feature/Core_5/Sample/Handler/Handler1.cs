using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

public class Handler1 :
    IHandleMessages<HandlerMessage>
{
    static ILog log = LogManager.GetLogger<Handler1>();
    static Random random = new Random();

    public void Handle(HandlerMessage message)
    {
        var milliseconds = random.Next(100, 1000);
        log.Info($"Message received going to Thread.Sleep({milliseconds}ms)");
        Thread.Sleep(milliseconds);
    }
}
