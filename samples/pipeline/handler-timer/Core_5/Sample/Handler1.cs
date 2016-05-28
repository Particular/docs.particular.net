using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

#region handler
public class Handler1 : IHandleMessages<Message>
{
    static ILog logger = LogManager.GetLogger<Handler1>();
    static Random random = new Random();

    public void Handle(Message message)
    {
        var milliseconds = random.Next(100, 1000);
        logger.InfoFormat("Message received going to Thread.Sleep({0}ms)", milliseconds);
        Thread.Sleep(milliseconds);
    }
}
#endregion