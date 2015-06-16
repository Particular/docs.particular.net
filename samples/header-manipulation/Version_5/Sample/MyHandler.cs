using System.Linq;
using NServiceBus;
using NServiceBus.Logging;

#region handler

public class MyHandler : IHandleMessages<MyMessage>
{
    IBus bus;
    static ILog logger = LogManager.GetLogger(typeof(MyHandler));

    public MyHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyMessage message)
    {
        logger.Info("Hello from MyHandler");
        foreach (string line in bus.CurrentMessageContext
            .Headers.OrderBy(x => x.Key)
            .Select(x => string.Format("Key={0}, Value={1}", x.Key, x.Value)))
        {
            logger.Info(line);
        }
    }
}

#endregion