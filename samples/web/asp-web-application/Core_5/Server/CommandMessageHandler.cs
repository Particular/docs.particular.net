using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class CommandMessageHandler :
    IHandleMessages<Command>
{
    static ILog log = LogManager.GetLogger<CommandMessageHandler>();
    IBus bus;

    public CommandMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(Command message)
    {
        log.Info("Hello from CommandMessageHandler");

        if (message.Id%2 == 0)
        {
            bus.Return(ErrorCodes.Fail);
        }
        else
        {
            bus.Return(ErrorCodes.None);
        }
    }
}
#endregion