using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

public class Handler2 : IHandleMessages<Message>
{
    static ILog logger = LogManager.GetLogger(typeof(Handler2));
    static Random random = new Random();

    public void Handle(Message message)
    {
        int milliseconds = random.Next(100, 1000);
        logger.InfoFormat("Message received going to Thread.Sleep({0}ms)", milliseconds);
        Thread.Sleep(milliseconds);
    }
}
