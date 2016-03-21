using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

#region Handler
public class CommandMessageHandler : IHandleMessages<Command>
{
    static ILog log = LogManager.GetLogger<CommandMessageHandler>();

    public async Task Handle(Command message, IMessageHandlerContext context)
    {
        log.Info("Hello from CommandMessageHandler");

        if (message.Id % 2 == 0)
        {
            await context.Reply(ErrorCodes.Fail);
        }
        else
        {
            await context.Reply(ErrorCodes.None);
        }
    }
}
#endregion