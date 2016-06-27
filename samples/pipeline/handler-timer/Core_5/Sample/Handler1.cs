using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

#region handler
public class Handler1 : IHandleMessages<Message>
{
    static ILog log = LogManager.GetLogger<Handler1>();
    static Random random = new Random();

    public void Handle(Message message)
    {
        var milliseconds = random.Next(100, 1000);
        log.Info($"Message received going to Thread.Sleep({milliseconds}ms)");
        Thread.Sleep(milliseconds);
    }
}
#endregion