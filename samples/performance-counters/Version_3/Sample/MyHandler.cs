using System;
using System.Threading;
using log4net;
using NServiceBus;

#region handler
public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger(typeof(MyHandler));

    static Random random = new Random();

    public void Handle(MyMessage message)
    {
        int sleepTime = random.Next(1, 300);
        Thread.Sleep(sleepTime);
        logger.Info(string.Format("Hello from MyHandler. Slept for {0}ms", sleepTime));
    }
}
#endregion