using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

public class Handler2 :
    IHandleMessages<Message>
{
    static ILog log = LogManager.GetLogger<Handler2>();

    static Random random = new Random();

    public void Handle(Message message)
    {
        var milliseconds = random.Next(100, 1000);
        log.Info($"Message received going to Thread.Sleep({milliseconds}ms)");
        Thread.Sleep(milliseconds);
    }
}