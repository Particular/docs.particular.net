using System.Linq;
using NServiceBus;
using NServiceBus.Logging;

#region handler

public class MyHandler :
    IHandleMessages<MyMessage>
{
    IBus bus;
    static ILog log = LogManager.GetLogger(typeof(MyHandler));

    public MyHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyMessage message)
    {
        log.Info("Hello from MyHandler");
        foreach (var line in bus.CurrentMessageContext
            .Headers.OrderBy(x => x.Key)
            .Select(x => $"Key={x.Key}, Value={x.Value}"))
        {
            log.Info(line);
        }
    }
}

#endregion