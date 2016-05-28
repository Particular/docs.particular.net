using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

#region Handler

public class CommandMessageHandler : IHandleMessages<Command>
{
    static ILog log = LogManager.GetLogger<CommandMessageHandler>();

    public Task Handle(Command message, IMessageHandlerContext context)
    {
        log.Info("Hello from CommandMessageHandler");
        Task reply;
        if (message.Id%2 == 0)
        {
            reply = context.Reply(ErrorCodes.Fail);
        }
        else
        {
            reply = context.Reply(ErrorCodes.None);
        }
        return reply;
    }
}

#endregion