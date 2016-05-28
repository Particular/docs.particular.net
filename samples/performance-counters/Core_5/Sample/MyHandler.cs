using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

#region handler
public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger<MyHandler>();

    static Random random = new Random();

    public void Handle(MyMessage message)
    {
        var sleepTime = random.Next(1, 1000);
        Thread.Sleep(sleepTime);
        logger.InfoFormat("Hello from MyHandler. Slept for {0}ms", sleepTime);
    }
}
#endregion