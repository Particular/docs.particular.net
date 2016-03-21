using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region CommandMessageHandler
public class CommandMessageHandler : IHandleMessages<Command>
{
    static ILog log = LogManager.GetLogger<CommandMessageHandler>();

    public async Task Handle(Command message, IMessageHandlerContext context)
    {
        log.Info("Hello from CommandMessageHandler");

        if (message.Id % 2 == 0)
        {
            log.Info("Returning Fail");
            await context.Reply(ErrorCodes.Fail);
        }
        else
        {
            log.Info("Returning None");
            await context.Reply(ErrorCodes.None);
        }
    }
}
#endregion
