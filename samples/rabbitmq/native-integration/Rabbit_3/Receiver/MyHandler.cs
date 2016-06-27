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
        log.Info($"Got `MyMessage` with id: {bus.CurrentMessageContext.Id}, property value: {message.SomeProperty}");
    }
}