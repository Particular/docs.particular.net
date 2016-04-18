using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger<MyHandler>();

    static Random random = new Random();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        int sleepTime = random.Next(1, 1000);
        await Task.Delay(sleepTime);
        logger.InfoFormat("Hello from MyHandler. Slept for {0}ms", sleepTime);
    }

}

#endregion