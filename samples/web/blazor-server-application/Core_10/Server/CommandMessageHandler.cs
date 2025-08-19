using Microsoft.Extensions.Logging;
using NServiceBus;

using System.Threading.Tasks;

#region Handler

public class CommandMessageHandler(ILogger<CommandMessageHandler> logger) :
    IHandleMessages<Command>
{
    public Task Handle(Command message, IMessageHandlerContext context)
    {
        logger.LogInformation("Hello from CommandMessageHandler");
        Task reply;

        if (message.Id % 2 == 0)
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
