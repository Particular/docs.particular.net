using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

#region handler
public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger(typeof(MyHandler));

    static Random random = new Random();
    public void Handle(MyMessage message)
    {
        int sleepTime = random.Next(1, 1000);
        Thread.Sleep(sleepTime);
        if (sleepTime%2 != 0)
        {
            throw new Exception();
        }
        logger.InfoFormat("Hello from MyHandler. Slept for {0}ms", sleepTime);
    }
}
#endregion