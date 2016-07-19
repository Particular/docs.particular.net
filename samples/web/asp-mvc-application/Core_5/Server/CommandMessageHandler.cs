using NServiceBus;
using NServiceBus.Logging;

#region CommandMessageHandler
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
            log.Info("Returning Fail");
            bus.Return(ErrorCodes.Fail);
        }
        else
        {
            log.Info("Returning None");
            bus.Return(ErrorCodes.None);
        }
    }
}
#endregion
