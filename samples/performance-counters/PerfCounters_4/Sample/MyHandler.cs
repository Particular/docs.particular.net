using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    static Random random = new Random();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var sleepTime = random.Next(1, 1000);
        await Task.Delay(sleepTime);
        log.Info($"Hello from MyHandler. Slept for {sleepTime} ms");
    }
}

#endregion