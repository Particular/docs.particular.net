using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

#region handler
public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger(typeof(MyHandler));

    static Random random = new Random();

    public void Handle(MyMessage message)
    {
        var sleepTime = random.Next(1, 1000);
        Thread.Sleep(sleepTime);
        log.Info($"Hello from MyHandler. Slept for {sleepTime}ms");
    }
}
#endregion