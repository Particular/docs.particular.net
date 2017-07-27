using Commands;
using NServiceBus;
using NServiceBus.Logging;

public class MyCommandHandler :
    IHandleMessages<MyCommand>
{
    static ILog log = LogManager.GetLogger<MyCommandHandler>();
    IBus bus;

    public MyCommandHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyCommand message)
    {
        log.Info($"Command received, id:{message.CommandId}");
    }
}