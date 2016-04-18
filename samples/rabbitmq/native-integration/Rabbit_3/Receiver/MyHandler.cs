using NServiceBus;
using NServiceBus.Logging;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();
    IBus bus;

    public MyHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyMessage message)
    {
        log.InfoFormat("Got `MyMessage` with id: {0}, property value: {1}", bus.CurrentMessageContext.Id, message.SomeProperty);
    }
}